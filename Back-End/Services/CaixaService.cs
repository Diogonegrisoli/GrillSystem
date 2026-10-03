using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Services;

public sealed class CaixaService(AppDbContext context)
{
    public Task<ResultadoPaginadoDto<Caixa>> Listar(PaginacaoDto pagina, CancellationToken ct) =>
        context.Caixas.AsNoTracking().OrderByDescending(x => x.DataAbertura).PaginarAsync(pagina, ct);

    public async Task<Caixa> Obter(int id, CancellationToken ct) =>
        await context.Caixas.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct)
        ?? throw new KeyNotFoundException($"Caixa {id} não encontrado.");

    public Task<ResultadoPaginadoDto<MovimentacaoCaixa>> Movimentacoes(
        int caixaId, PaginacaoDto pagina, CancellationToken ct) =>
        context.MovimentacoesCaixa.AsNoTracking().Where(x => x.CaixaId == caixaId)
            .OrderByDescending(x => x.DataMovimentacao).ThenByDescending(x => x.Id)
            .PaginarAsync(pagina, ct);

    public async Task<Caixa> Abrir(AbrirCaixaDto dto, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(dto.Descricao))
            throw new ValidationException("Informe a descrição do caixa.");
        var caixa = new Caixa
        {
            Descricao = dto.Descricao.Trim(),
            Tipo = dto.Tipo ?? throw new ValidationException("Informe o tipo do caixa."),
            SaldoInicial = dto.SaldoInicial,
            SaldoFinal = dto.SaldoInicial,
            DataAbertura = DateTime.UtcNow
        };
        context.Caixas.Add(caixa);
        await context.SaveChangesAsync(ct);
        return caixa;
    }

    public async Task<Caixa> Fechar(int caixaId, CancellationToken ct)
    {
        int alterados = await context.Caixas
            .Where(x => x.Id == caixaId && x.Situacao == SituacaoCaixa.Aberto)
            .ExecuteUpdateAsync(x => x
                .SetProperty(c => c.Situacao, SituacaoCaixa.Fechado)
                .SetProperty(c => c.DataFechamento, DateTime.UtcNow), ct);
        if (alterados == 0)
        {
            if (!await context.Caixas.AnyAsync(x => x.Id == caixaId, ct))
                throw new KeyNotFoundException($"Caixa {caixaId} não encontrado.");
            throw new ConflitoNegocioException("O caixa já está fechado.");
        }
        return await Obter(caixaId, ct);
    }

    public async Task<MovimentacaoCaixa> Liquidar(int caixaId, LiquidarParcelaDto dto, CancellationToken ct)
    {
        var tipo = dto.Tipo ?? throw new ValidationException("Informe o tipo de movimentação.");
        if (!Enum.IsDefined(tipo))
            throw new ValidationException("Tipo de movimentação inválido.");
        await using var transacao = await context.Database.BeginTransactionAsync(ct);
        DateOnly hoje = DateOnly.FromDateTime(DateTime.Today);
        decimal valor;
        int? parcelaPagarId = null;
        int? parcelaReceberId = null;

        if (tipo == TipoMovimentacaoCaixa.Saida)
        {
            var parcela = await context.ContasPagarParceladas.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == dto.ParcelaId, ct)
                ?? throw new KeyNotFoundException("Parcela a pagar não encontrada.");
            if (parcela.Situacao != SituacaoParcela.Pendente)
                throw new ConflitoNegocioException("A parcela já foi liquidada ou cancelada.");
            valor = parcela.ValorParcela;
            parcelaPagarId = parcela.Id;
            int atualizados = await context.ContasPagarParceladas
                .Where(x => x.Id == parcela.Id && x.Situacao == SituacaoParcela.Pendente)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.Situacao, SituacaoParcela.Liquidada)
                    .SetProperty(p => p.DataPagamento, hoje), ct);
            if (atualizados == 0) throw new ConflitoNegocioException("Parcela alterada por outra operação.");
        }
        else
        {
            var parcela = await context.ContasReceberParceladas.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == dto.ParcelaId, ct)
                ?? throw new KeyNotFoundException("Parcela a receber não encontrada.");
            if (parcela.Situacao != SituacaoParcela.Pendente)
                throw new ConflitoNegocioException("A parcela já foi liquidada ou cancelada.");
            valor = parcela.ValorParcela;
            parcelaReceberId = parcela.Id;
            int atualizados = await context.ContasReceberParceladas
                .Where(x => x.Id == parcela.Id && x.Situacao == SituacaoParcela.Pendente)
                .ExecuteUpdateAsync(x => x.SetProperty(p => p.Situacao, SituacaoParcela.Liquidada)
                    .SetProperty(p => p.DataRecebimento, hoje), ct);
            if (atualizados == 0) throw new ConflitoNegocioException("Parcela alterada por outra operação.");
        }

        if (valor <= 0)
            throw new RegraNegocioException("A parcela deve ter valor maior que zero.");

        int caixaAtualizado = tipo == TipoMovimentacaoCaixa.Entrada
            ? await context.Caixas.Where(x => x.Id == caixaId && x.Situacao == SituacaoCaixa.Aberto)
                .ExecuteUpdateAsync(x => x.SetProperty(c => c.SaldoFinal, c => c.SaldoFinal + valor), ct)
            : await context.Caixas.Where(x => x.Id == caixaId && x.Situacao == SituacaoCaixa.Aberto
                    && x.SaldoFinal >= valor)
                .ExecuteUpdateAsync(x => x.SetProperty(c => c.SaldoFinal, c => c.SaldoFinal - valor), ct);
        if (caixaAtualizado == 0)
            throw new RegraNegocioException("Caixa fechado, inexistente ou sem saldo suficiente.");

        var movimento = new MovimentacaoCaixa
        {
            CaixaId = caixaId,
            TipoMovimentacao = tipo,
            Valor = valor,
            DataMovimentacao = DateTime.UtcNow,
            Descricao = dto.Descricao.Trim(),
            ParcelaPagarId = parcelaPagarId,
            ParcelaReceberId = parcelaReceberId
        };
        context.MovimentacoesCaixa.Add(movimento);
        await context.SaveChangesAsync(ct);

        if (parcelaPagarId.HasValue)
        {
            int contaId = await context.ContasPagarParceladas.Where(x => x.Id == parcelaPagarId.Value)
                .Select(x => x.ContaPagarId).SingleAsync(ct);
            if (!await context.ContasPagarParceladas.AnyAsync(
                    x => x.ContaPagarId == contaId && x.Situacao == SituacaoParcela.Pendente, ct))
            {
                await context.ContasPagar.Where(x => x.Id == contaId)
                    .ExecuteUpdateAsync(x => x.SetProperty(c => c.Status, StatusContaPagar.Pago)
                        .SetProperty(c => c.DataPagamento, DateTime.UtcNow), ct);
            }
        }
        else
        {
            int contaId = await context.ContasReceberParceladas.Where(x => x.Id == parcelaReceberId!.Value)
                .Select(x => x.ContaReceberId).SingleAsync(ct);
            if (!await context.ContasReceberParceladas.AnyAsync(
                    x => x.ContaReceberId == contaId && x.Situacao == SituacaoParcela.Pendente, ct))
            {
                await context.ContasReceber.Where(x => x.Id == contaId)
                    .ExecuteUpdateAsync(x => x.SetProperty(c => c.StatusPagamento, StatusPagamento.Pago)
                        .SetProperty(c => c.DataRecebimento, hoje), ct);
            }
        }

        await transacao.CommitAsync(ct);
        return movimento;
    }
}

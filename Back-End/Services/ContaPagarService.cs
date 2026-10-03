using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using GrillSystem.Validacao;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Services;

public class ContaPagarService
{
    private readonly AppDbContext _context;

    public ContaPagarService(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<ContaPagar>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.ContasPagar.AsNoTracking()
            .OrderBy(x => x.Status).ThenBy(x => x.DataVencimento)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<ContaPagar> GetId(int id, CancellationToken cancellationToken = default) =>
        await _context.ContasPagar.AsNoTracking().Include(x => x.Parcelas)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"A conta a pagar com o id {id} não foi localizada.");

    public async Task<ContaPagar> Create(
        ContaPagarDto data,
        CancellationToken cancellationToken = default)
    {
        DateTime emissao = Validacoes.DataEmissao(data.DataEmissao);
        TipoPagamento tipoPagamento = data.TipoPagamento
            ?? throw new ValidationException("O tipo de pagamento deve ser informado.");
        if (data.DataPagamento.HasValue)
        {
            throw new RegraNegocioException("Registre o pagamento da parcela pelo caixa.");
        }
        Validacoes.PeriodoValido(
            emissao,
            data.DataVencimento,
            data.DataPagamento,
            "data de emissão",
            "data de vencimento",
            "data de pagamento");
        if (data.DataPagamento.HasValue && data.DataPagamento.Value.Date > DateTime.Today)
        {
            throw new RegraNegocioException("A data de pagamento não pode ser futura.");
        }

        decimal total = await ObterTotalPedido(data.PedidoCompraId, cancellationToken);
        if (await _context.ContasPagar.AnyAsync(
                x => x.PedidoCompraId == data.PedidoCompraId,
                cancellationToken))
        {
            throw new ConflitoNegocioException("O pedido já possui uma conta a pagar.");
        }

        var pedido = await _context.PedidosCompra.AsNoTracking()
            .SingleAsync(x => x.Id == data.PedidoCompraId, cancellationToken);
        var conta = new ContaPagar(
            total,
            tipoPagamento,
            emissao,
            data.DataVencimento,
            data.DataPagamento,
            data.PedidoCompraId)
        {
            Observacao = data.Observacao.Trim()
        };
        foreach (var (numero, valor, vencimento) in Parcelamento.Gerar(
            total, pedido.Parcelas, DateOnly.FromDateTime(data.DataVencimento)))
        {
            conta.Parcelas.Add(new ContaPagarParcelada
            {
                NumeroParcela = numero,
                ValorParcela = valor,
                DataVencimento = vencimento
            });
        }
        _context.ContasPagar.Add(conta);
        await _context.SaveChangesAsync(cancellationToken);
        return conta;
    }

    public async Task<ContaPagar> Update(
        int id,
        ContaPagarUpdateDto data,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        var conta = await _context.ContasPagar.Include(x => x.Parcelas)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A conta a pagar com o id {id} não foi localizada.");
        TipoPagamento tipoPagamento = data.TipoPagamento
            ?? throw new ValidationException("O tipo de pagamento deve ser informado.");
        if (data.DataPagamento.HasValue || conta.Status == StatusContaPagar.Pago)
        {
            throw new RegraNegocioException("Pagamentos são registrados pelo caixa e não podem ser editados diretamente.");
        }
        if (conta.Parcelas.Any(x => x.Situacao != SituacaoParcela.Pendente))
        {
            throw new RegraNegocioException("Uma conta com parcela liquidada não pode ser alterada.");
        }
        if (conta.PedidoCompraId != data.PedidoCompraId)
        {
            throw new RegraNegocioException("A conta não pode ser transferida para outro pedido.");
        }
        Validacoes.PeriodoValido(
            conta.DataEmissao,
            data.DataVencimento,
            data.DataPagamento,
            "data de emissão",
            "data de vencimento",
            "data de pagamento");
        if (data.DataPagamento.HasValue && data.DataPagamento.Value.Date > DateTime.Today)
        {
            throw new RegraNegocioException("A data de pagamento não pode ser futura.");
        }
        if (await _context.ContasPagar.AnyAsync(
                x => x.PedidoCompraId == data.PedidoCompraId && x.Id != id,
                cancellationToken))
        {
            throw new ConflitoNegocioException("O pedido já possui outra conta a pagar.");
        }

        conta.Valor = await ObterTotalPedido(data.PedidoCompraId, cancellationToken);
        conta.TipoPagamento = tipoPagamento;
        conta.Observacao = data.Observacao.Trim();
        conta.DataVencimento = data.DataVencimento;
        conta.DataPagamento = null;
        conta.Status = StatusContaPagar.Pendente;
        var pedido = await _context.PedidosCompra.AsNoTracking()
            .SingleAsync(x => x.Id == data.PedidoCompraId, cancellationToken);
        var plano = Parcelamento.Gerar(
            conta.Valor, pedido.Parcelas, DateOnly.FromDateTime(data.DataVencimento));
        if (conta.Parcelas.Count != plano.Count)
        {
            _context.ContasPagarParceladas.RemoveRange(conta.Parcelas);
            conta.Parcelas.Clear();
            await _context.SaveChangesAsync(cancellationToken);
            foreach (var (numero, valor, vencimento) in plano)
            {
                conta.Parcelas.Add(new ContaPagarParcelada
                {
                    NumeroParcela = numero, ValorParcela = valor, DataVencimento = vencimento
                });
            }
        }
        else
        {
            foreach (var parcela in conta.Parcelas)
            {
                var novo = plano[parcela.NumeroParcela - 1];
                parcela.ValorParcela = novo.Valor;
                parcela.DataVencimento = novo.Vencimento;
            }
        }
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return conta;
    }

    public async Task<ContaPagar> Delete(int id, CancellationToken cancellationToken = default)
    {
        var conta = await _context.ContasPagar.Include(x => x.Parcelas)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A conta a pagar com o id {id} não foi localizada.");
        if (conta.Status == StatusContaPagar.Pago)
        {
            throw new RegraNegocioException("Uma conta paga não pode ser excluída.");
        }
        if (conta.Parcelas.Any(x => x.Situacao != SituacaoParcela.Pendente))
            throw new RegraNegocioException("A conta possui pagamentos e não pode ser excluída.");
        _context.ContasPagarParceladas.RemoveRange(conta.Parcelas);
        _context.ContasPagar.Remove(conta);
        await _context.SaveChangesAsync(cancellationToken);
        return conta;
    }

    private async Task<decimal> ObterTotalPedido(int pedidoId, CancellationToken cancellationToken)
    {
        decimal? total = await _context.PedidosCompra
            .Where(x => x.Id == pedidoId)
            .Select(x => (decimal?)x.ValorTotal)
            .FirstOrDefaultAsync(cancellationToken);
        if (!total.HasValue)
        {
            throw new KeyNotFoundException($"O pedido de compra com o id {pedidoId} não foi localizado.");
        }
        if (total.Value <= 0)
        {
            throw new RegraNegocioException("Inclua os itens do pedido antes de gerar a conta a pagar.");
        }
        return total.Value;
    }
}

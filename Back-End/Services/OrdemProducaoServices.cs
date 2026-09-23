using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using GrillSystem.Validacao;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Services;

public class OrdemProducaoServices
{
    private readonly AppDbContext _context;

    public OrdemProducaoServices(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<OrdemProducao>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.OrdensProducao
            .AsNoTracking()
            .OrderByDescending(x => x.DataInicio)
            .ThenByDescending(x => x.Id)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<OrdemProducao> GetId(
        int id,
        CancellationToken cancellationToken = default) =>
        await _context.OrdensProducao
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"A ordem de produção com o id {id} não foi localizada.");

    public async Task<OrdemProducao> Create(
        OrdemProducaoDto data,
        CancellationToken cancellationToken = default)
    {
        Validacoes.DataObrigatoria(data.DataInicio, "A data de início");
        TipoOrdem tipo = data.Tipo
            ?? throw new ValidationException("O tipo da ordem deve ser informado.");
        var ordem = new OrdemProducao(data.Quantidade, data.DataInicio, null, tipo);
        _context.OrdensProducao.Add(ordem);
        await _context.SaveChangesAsync(cancellationToken);
        return ordem;
    }

    public async Task<OrdemProducao> Update(
        int id,
        OrdemProducaoUpdateDto data,
        CancellationToken cancellationToken = default)
    {
        var ordem = await _context.OrdensProducao
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A ordem de produção com o id {id} não foi localizada.");

        if (ordem.Status == StatusOrdem.Finalizado)
        {
            throw new RegraNegocioException("Uma ordem finalizada não pode ser alterada.");
        }
        Validacoes.DataObrigatoria(data.DataInicio, "A data de início");
        TipoOrdem tipo = data.Tipo
            ?? throw new ValidationException("O tipo da ordem deve ser informado.");
        StatusOrdem status = data.Status
            ?? throw new ValidationException("O status da ordem deve ser informado.");
        if (status == StatusOrdem.Finalizado)
        {
            throw new RegraNegocioException(
                "Use a operação específica de finalização para concluir a ordem.");
        }
        if (ordem.Status == StatusOrdem.EmAndamento && status == StatusOrdem.Pendente)
        {
            throw new RegraNegocioException("Uma ordem em andamento não pode voltar para pendente.");
        }
        if (ordem.Status == StatusOrdem.Pendente && status == StatusOrdem.EmAndamento)
        {
            await ValidarProdutoEComposicao(id, cancellationToken);
        }

        if (ordem.Status == StatusOrdem.Pendente)
        {
            ordem.Quantidade = data.Quantidade;
            ordem.DataInicio = data.DataInicio;
            ordem.Tipo = tipo;
        }
        else if (ordem.Quantidade != data.Quantidade ||
                 ordem.DataInicio != data.DataInicio ||
                 ordem.Tipo != tipo)
        {
            throw new RegraNegocioException(
                "Quantidade, data e tipo só podem ser alterados enquanto a ordem estiver pendente.");
        }

        ordem.Status = status;
        await _context.SaveChangesAsync(cancellationToken);
        return ordem;
    }

    public async Task<OrdemProducao> Finalizar(
        int id,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        var ordem = await _context.OrdensProducao
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A ordem de produção com o id {id} não foi localizada.");
        if (ordem.Status != StatusOrdem.EmAndamento)
        {
            throw new RegraNegocioException(
                "Somente uma ordem em andamento pode ser finalizada.");
        }

        var vinculo = await _context.ProdutosOrdensProducao
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.OrdemProducaoId == id, cancellationToken)
            ?? throw new RegraNegocioException("A ordem não possui um produto associado.");
        var composicao = await _context.ProdutosMateriasPrimas
            .AsNoTracking()
            .Where(x => x.ProdutoId == vinculo.ProdutoId)
            .OrderBy(x => x.MateriaPrimaId)
            .ToListAsync(cancellationToken);
        if (composicao.Count == 0)
        {
            throw new RegraNegocioException("O produto não possui uma composição cadastrada.");
        }
        if (composicao.Any(x => x.QuantidadeNecessaria <= 0))
        {
            throw new RegraNegocioException(
                "A composição possui matéria-prima sem quantidade necessária válida. Revise a ficha técnica do produto.");
        }

        DateOnly dataFinalizacao = DateOnly.FromDateTime(DateTime.Today);
        if (dataFinalizacao < ordem.DataInicio)
        {
            throw new RegraNegocioException(
                "A ordem não pode ser finalizada antes de sua data de início.");
        }

        int ordemReservada = await _context.OrdensProducao
            .Where(x => x.Id == id && x.Status == StatusOrdem.EmAndamento)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(x => x.Status, StatusOrdem.Finalizado)
                    .SetProperty(x => x.DataFim, dataFinalizacao),
                cancellationToken);
        if (ordemReservada == 0)
        {
            throw new ConflitoNegocioException(
                "A ordem já foi finalizada ou alterada por outra operação.");
        }

        foreach (var item in composicao)
        {
            decimal consumo = item.QuantidadeNecessaria * ordem.Quantidade;
            int atualizados = await _context.MateriasPrimas
                .Where(x => x.Id == item.MateriaPrimaId && x.Quantidade >= consumo)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(
                        x => x.Quantidade,
                        x => x.Quantidade - consumo),
                    cancellationToken);
            if (atualizados == 0)
            {
                throw new RegraNegocioException(
                    $"Estoque insuficiente para a matéria-prima {item.MateriaPrimaId}.");
            }

            _context.MovimentacoesEstoque.Add(new MovimentacaoEstoque(
                TipoMovimentacao.Saida,
                consumo,
                0,
                dataFinalizacao,
                $"Consumo da ordem de produção #{ordem.Id}",
                item.MateriaPrimaId));
        }

        int produtoAtualizado = await _context.Produtos
            .Where(x => x.Id == vinculo.ProdutoId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    x => x.Quantidade,
                    x => x.Quantidade + ordem.Quantidade),
                cancellationToken);
        if (produtoAtualizado == 0)
        {
            throw new KeyNotFoundException($"O produto com o id {vinculo.ProdutoId} não foi localizado.");
        }

        ordem.Status = StatusOrdem.Finalizado;
        ordem.DataFim = dataFinalizacao;
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return ordem;
    }

    public async Task<OrdemProducao> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ordem = await _context.OrdensProducao
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A ordem de produção com o id {id} não foi localizada.");
        if (ordem.Status != StatusOrdem.Pendente)
        {
            throw new RegraNegocioException("Somente ordens pendentes podem ser excluídas.");
        }
        _context.OrdensProducao.Remove(ordem);
        await _context.SaveChangesAsync(cancellationToken);
        return ordem;
    }

    private async Task ValidarProdutoEComposicao(
        int ordemId,
        CancellationToken cancellationToken)
    {
        int? produtoId = await _context.ProdutosOrdensProducao
            .Where(x => x.OrdemProducaoId == ordemId)
            .Select(x => (int?)x.ProdutoId)
            .SingleOrDefaultAsync(cancellationToken);
        if (!produtoId.HasValue)
        {
            throw new RegraNegocioException(
                "Associe um produto antes de iniciar a ordem de produção.");
        }

        bool composicaoValida = await _context.ProdutosMateriasPrimas
            .AnyAsync(
                x => x.ProdutoId == produtoId.Value && x.QuantidadeNecessaria > 0,
                cancellationToken);
        bool composicaoInvalida = await _context.ProdutosMateriasPrimas
            .AnyAsync(
                x => x.ProdutoId == produtoId.Value && x.QuantidadeNecessaria <= 0,
                cancellationToken);
        if (!composicaoValida || composicaoInvalida)
        {
            throw new RegraNegocioException(
                "Cadastre uma ficha técnica válida para o produto antes de iniciar a produção.");
        }
    }
}

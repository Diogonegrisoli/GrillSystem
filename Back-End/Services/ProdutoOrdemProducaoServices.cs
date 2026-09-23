using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services;

public class ProdutoOrdemProducaoServices
{
    private readonly AppDbContext _context;

    public ProdutoOrdemProducaoServices(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<ProdutoOrdemProducao>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.ProdutosOrdensProducao
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<ProdutoOrdemProducao> GetId(
        int id,
        CancellationToken cancellationToken = default) =>
        await _context.ProdutosOrdensProducao
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException(
            $"O vínculo de produto e ordem com o id {id} não foi localizado.");

    public async Task<ProdutoOrdemProducao> Create(
        ProdutoOrdemProducaoDto data,
        CancellationToken cancellationToken = default)
    {
        await Validar(data, null, cancellationToken);
        var vinculo = new ProdutoOrdemProducao(data.ProdutoId, data.OrdemProducaoId);
        _context.ProdutosOrdensProducao.Add(vinculo);
        await _context.SaveChangesAsync(cancellationToken);
        return vinculo;
    }

    public async Task<ProdutoOrdemProducao> Update(
        int id,
        ProdutoOrdemProducaoDto data,
        CancellationToken cancellationToken = default)
    {
        var vinculo = await _context.ProdutosOrdensProducao
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException(
                $"O vínculo de produto e ordem com o id {id} não foi localizado.");
        await Validar(data, id, cancellationToken);
        vinculo.ProdutoId = data.ProdutoId;
        vinculo.OrdemProducaoId = data.OrdemProducaoId;
        await _context.SaveChangesAsync(cancellationToken);
        return vinculo;
    }

    public async Task<ProdutoOrdemProducao> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        var vinculo = await _context.ProdutosOrdensProducao
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException(
                $"O vínculo de produto e ordem com o id {id} não foi localizado.");
        var status = await _context.OrdensProducao
            .Where(x => x.Id == vinculo.OrdemProducaoId)
            .Select(x => x.Status)
            .FirstAsync(cancellationToken);
        if (status != StatusOrdem.Pendente)
        {
            throw new RegraNegocioException(
                "O produto não pode ser removido de uma ordem já iniciada.");
        }
        _context.ProdutosOrdensProducao.Remove(vinculo);
        await _context.SaveChangesAsync(cancellationToken);
        return vinculo;
    }

    private async Task Validar(
        ProdutoOrdemProducaoDto data,
        int? ignorarId,
        CancellationToken cancellationToken)
    {
        if (!await _context.Produtos.AnyAsync(x => x.Id == data.ProdutoId, cancellationToken))
        {
            throw new KeyNotFoundException($"O produto com o id {data.ProdutoId} não foi localizado.");
        }
        var ordem = await _context.OrdensProducao
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == data.OrdemProducaoId, cancellationToken)
            ?? throw new KeyNotFoundException(
                $"A ordem de produção com o id {data.OrdemProducaoId} não foi localizada.");
        if (ordem.Status != StatusOrdem.Pendente)
        {
            throw new RegraNegocioException("O produto só pode ser definido em uma ordem pendente.");
        }

        bool jaAssociada = await _context.ProdutosOrdensProducao.AnyAsync(
            x => x.OrdemProducaoId == data.OrdemProducaoId &&
                 (!ignorarId.HasValue || x.Id != ignorarId.Value),
            cancellationToken);
        if (jaAssociada)
        {
            throw new ConflitoNegocioException("A ordem de produção já possui um produto.");
        }
    }
}

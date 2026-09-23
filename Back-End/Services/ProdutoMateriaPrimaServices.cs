using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services;

public class ProdutoMateriaPrimaServices
{
    private readonly AppDbContext _context;

    public ProdutoMateriaPrimaServices(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<ProdutoMateriaPrima>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.ProdutosMateriasPrimas
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<ProdutoMateriaPrima> GetId(
        int id,
        CancellationToken cancellationToken = default) =>
        await _context.ProdutosMateriasPrimas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException(
            $"A composição de produto com o id {id} não foi localizada.");

    public async Task<ProdutoMateriaPrima> Create(
        ProdutoMateriaPrimaDto data,
        CancellationToken cancellationToken = default)
    {
        await ValidarReferencias(data, null, cancellationToken);

        var composicao = new ProdutoMateriaPrima(
            data.ProdutoId,
            data.MateriaPrimaId,
            data.QuantidadeNecessaria);

        _context.ProdutosMateriasPrimas.Add(composicao);
        await _context.SaveChangesAsync(cancellationToken);
        return composicao;
    }

    public async Task<ProdutoMateriaPrima> Update(
        int id,
        ProdutoMateriaPrimaDto data,
        CancellationToken cancellationToken = default)
    {
        var composicao = await _context.ProdutosMateriasPrimas
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException(
                $"A composição de produto com o id {id} não foi localizada.");

        await ValidarReferencias(data, id, cancellationToken);
        composicao.ProdutoId = data.ProdutoId;
        composicao.MateriaPrimaId = data.MateriaPrimaId;
        composicao.QuantidadeNecessaria = data.QuantidadeNecessaria;
        await _context.SaveChangesAsync(cancellationToken);
        return composicao;
    }

    public async Task<ProdutoMateriaPrima> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        var composicao = await _context.ProdutosMateriasPrimas
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException(
                $"A composição de produto com o id {id} não foi localizada.");

        _context.ProdutosMateriasPrimas.Remove(composicao);
        await _context.SaveChangesAsync(cancellationToken);
        return composicao;
    }

    private async Task ValidarReferencias(
        ProdutoMateriaPrimaDto data,
        int? ignorarId,
        CancellationToken cancellationToken)
    {
        if (!await _context.Produtos.AnyAsync(x => x.Id == data.ProdutoId, cancellationToken))
        {
            throw new KeyNotFoundException($"O produto com o id {data.ProdutoId} não foi localizado.");
        }

        if (!await _context.MateriasPrimas.AnyAsync(x => x.Id == data.MateriaPrimaId, cancellationToken))
        {
            throw new KeyNotFoundException(
                $"A matéria-prima com o id {data.MateriaPrimaId} não foi localizada.");
        }

        bool duplicada = await _context.ProdutosMateriasPrimas.AnyAsync(
            x => x.ProdutoId == data.ProdutoId &&
                 x.MateriaPrimaId == data.MateriaPrimaId &&
                 (!ignorarId.HasValue || x.Id != ignorarId.Value),
            cancellationToken);

        if (duplicada)
        {
            throw new ConflitoNegocioException(
                "A matéria-prima já faz parte da composição deste produto.");
        }
    }
}

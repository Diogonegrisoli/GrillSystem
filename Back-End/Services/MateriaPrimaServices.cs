using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services;

public class MateriaPrimaServices
{
    private readonly AppDbContext _context;

    public MateriaPrimaServices(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<MateriaPrima>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.MateriasPrimas.AsNoTracking().OrderBy(x => x.Codigo)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<MateriaPrima> GetId(int id, CancellationToken cancellationToken = default) =>
        await _context.MateriasPrimas.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"A matéria-prima com o id {id} não foi localizada.");

    public async Task<MateriaPrima> Create(
        MateriaPrimaDto data,
        CancellationToken cancellationToken = default)
    {
        string codigo = data.Codigo.Trim();
        await ValidarCodigo(codigo, null, cancellationToken);
        var materiaPrima = new MateriaPrima(
            codigo,
            data.Descricao.Trim(),
            0,
            data.QuantidadeMinima,
            data.UnidadeMedida);
        _context.MateriasPrimas.Add(materiaPrima);
        await _context.SaveChangesAsync(cancellationToken);
        return materiaPrima;
    }

    public async Task<MateriaPrima> Update(
        int id,
        MateriaPrimaDto data,
        CancellationToken cancellationToken = default)
    {
        var materiaPrima = await _context.MateriasPrimas
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A matéria-prima com o id {id} não foi localizada.");
        string codigo = data.Codigo.Trim();
        await ValidarCodigo(codigo, id, cancellationToken);
        materiaPrima.Codigo = codigo;
        materiaPrima.Descricao = data.Descricao.Trim();
        materiaPrima.QuantidadeMinima = data.QuantidadeMinima;
        materiaPrima.UnidadeMedida = data.UnidadeMedida;
        await _context.SaveChangesAsync(cancellationToken);
        return materiaPrima;
    }

    public async Task<MateriaPrima> Delete(int id, CancellationToken cancellationToken = default)
    {
        var materiaPrima = await _context.MateriasPrimas
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A matéria-prima com o id {id} não foi localizada.");
        bool utilizada = await _context.ProdutosMateriasPrimas.AnyAsync(x => x.MateriaPrimaId == id, cancellationToken)
            || await _context.MovimentacoesEstoque.AnyAsync(x => x.MateriaPrimaId == id, cancellationToken)
            || await _context.PedidosCompraMateriasPrimas.AnyAsync(x => x.MateriaPrimaId == id, cancellationToken);
        if (utilizada)
        {
            throw new ConflitoNegocioException(
                "A matéria-prima possui histórico ou composição e não pode ser excluída.");
        }
        _context.MateriasPrimas.Remove(materiaPrima);
        await _context.SaveChangesAsync(cancellationToken);
        return materiaPrima;
    }

    private async Task ValidarCodigo(
        string codigo,
        int? ignorarId,
        CancellationToken cancellationToken)
    {
        if (await _context.MateriasPrimas.AnyAsync(
                x => x.Codigo == codigo && (!ignorarId.HasValue || x.Id != ignorarId),
                cancellationToken))
        {
            throw new ConflitoNegocioException("O código da matéria-prima já está cadastrado.");
        }
    }
}

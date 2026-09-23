using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Services;

public class CategoriaFinanceiraService
{
    private readonly AppDbContext _context;

    public CategoriaFinanceiraService(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<CategoriaFinanceira>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.CategoriasFinanceiras.AsNoTracking().OrderBy(x => x.Nome)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<CategoriaFinanceira> GetId(int id, CancellationToken cancellationToken = default) =>
        await _context.CategoriasFinanceiras.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"A categoria financeira com o id {id} não foi localizada.");

    public async Task<CategoriaFinanceira> Create(
        CategoriaFinanceiraDto data,
        CancellationToken cancellationToken = default)
    {
        string nome = data.Nome.Trim();
        TipoCategoria tipo = data.Tipo
            ?? throw new ValidationException("O tipo da categoria deve ser informado.");
        await ValidarDuplicidade(nome, tipo, null, cancellationToken);
        var categoria = new CategoriaFinanceira(nome, tipo);
        _context.CategoriasFinanceiras.Add(categoria);
        await _context.SaveChangesAsync(cancellationToken);
        return categoria;
    }

    public async Task<CategoriaFinanceira> Update(
        int id,
        CategoriaFinanceiraDto data,
        CancellationToken cancellationToken = default)
    {
        var categoria = await _context.CategoriasFinanceiras
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A categoria financeira com o id {id} não foi localizada.");
        string nome = data.Nome.Trim();
        TipoCategoria tipo = data.Tipo
            ?? throw new ValidationException("O tipo da categoria deve ser informado.");
        await ValidarDuplicidade(nome, tipo, id, cancellationToken);
        categoria.Nome = nome;
        categoria.Tipo = tipo;
        await _context.SaveChangesAsync(cancellationToken);
        return categoria;
    }

    public async Task<CategoriaFinanceira> Delete(int id, CancellationToken cancellationToken = default)
    {
        var categoria = await _context.CategoriasFinanceiras
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A categoria financeira com o id {id} não foi localizada.");
        if (await _context.Lancamentos.AnyAsync(x => x.CategoriaFinanceiraId == id, cancellationToken))
        {
            throw new ConflitoNegocioException("A categoria possui lançamentos e não pode ser excluída.");
        }
        _context.CategoriasFinanceiras.Remove(categoria);
        await _context.SaveChangesAsync(cancellationToken);
        return categoria;
    }

    private async Task ValidarDuplicidade(
        string nome,
        TipoCategoria tipo,
        int? ignorarId,
        CancellationToken cancellationToken)
    {
        bool existe = await _context.CategoriasFinanceiras.AnyAsync(
            x => x.Nome == nome && x.Tipo == tipo && (!ignorarId.HasValue || x.Id != ignorarId),
            cancellationToken);
        if (existe)
        {
            throw new ConflitoNegocioException("Já existe uma categoria com o mesmo nome e tipo.");
        }
    }
}

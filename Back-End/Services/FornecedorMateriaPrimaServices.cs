using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services;

public class FornecedorMateriaPrimaServices
{
    private readonly AppDbContext _context;

    public FornecedorMateriaPrimaServices(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<FornecedorMateriaPrima>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.FornecedoresMateriaPrima.AsNoTracking().OrderBy(x => x.Id)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<FornecedorMateriaPrima> GetId(
        int id,
        CancellationToken cancellationToken = default) =>
        await _context.FornecedoresMateriaPrima.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException(
            $"O vínculo de fornecedor e matéria-prima com o id {id} não foi localizado.");

    public async Task<FornecedorMateriaPrima> Create(
        FornecedorMateriaPrimaDto data,
        CancellationToken cancellationToken = default)
    {
        await Validar(data, null, cancellationToken);
        var vinculo = new FornecedorMateriaPrima(data.FornecedorId, data.MateriaPrimaId);
        _context.FornecedoresMateriaPrima.Add(vinculo);
        await _context.SaveChangesAsync(cancellationToken);
        return vinculo;
    }

    public async Task<FornecedorMateriaPrima> Update(
        int id,
        FornecedorMateriaPrimaDto data,
        CancellationToken cancellationToken = default)
    {
        var vinculo = await _context.FornecedoresMateriaPrima
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException(
                $"O vínculo de fornecedor e matéria-prima com o id {id} não foi localizado.");
        await Validar(data, id, cancellationToken);
        vinculo.FornecedorId = data.FornecedorId;
        vinculo.MateriaPrimaId = data.MateriaPrimaId;
        await _context.SaveChangesAsync(cancellationToken);
        return vinculo;
    }

    public async Task<FornecedorMateriaPrima> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        var vinculo = await _context.FornecedoresMateriaPrima
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException(
                $"O vínculo de fornecedor e matéria-prima com o id {id} não foi localizado.");
        _context.FornecedoresMateriaPrima.Remove(vinculo);
        await _context.SaveChangesAsync(cancellationToken);
        return vinculo;
    }

    private async Task Validar(
        FornecedorMateriaPrimaDto data,
        int? ignorarId,
        CancellationToken cancellationToken)
    {
        if (!await _context.Fornecedores.AnyAsync(x => x.Id == data.FornecedorId, cancellationToken))
        {
            throw new KeyNotFoundException($"O fornecedor com o id {data.FornecedorId} não foi localizado.");
        }
        if (!await _context.MateriasPrimas.AnyAsync(x => x.Id == data.MateriaPrimaId, cancellationToken))
        {
            throw new KeyNotFoundException(
                $"A matéria-prima com o id {data.MateriaPrimaId} não foi localizada.");
        }
        if (await _context.FornecedoresMateriaPrima.AnyAsync(
                x => x.FornecedorId == data.FornecedorId &&
                     x.MateriaPrimaId == data.MateriaPrimaId &&
                     (!ignorarId.HasValue || x.Id != ignorarId),
                cancellationToken))
        {
            throw new ConflitoNegocioException(
                "O fornecedor já está associado a esta matéria-prima.");
        }
    }
}

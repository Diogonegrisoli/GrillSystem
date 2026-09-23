using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services;

public class PedidoCompraMateriaPrimaService
{
    private readonly AppDbContext _context;

    public PedidoCompraMateriaPrimaService(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<PedidoCompraMateriaPrima>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.PedidosCompraMateriasPrimas
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<PedidoCompraMateriaPrima> GetId(
        int id,
        CancellationToken cancellationToken = default) =>
        await _context.PedidosCompraMateriasPrimas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"O item de compra com o id {id} não foi localizado.");

    public async Task<PedidoCompraMateriaPrima> Create(
        PedidoCompraMateriaPrimaDto data,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        await ValidarReferencias(data, null, cancellationToken);

        var item = new PedidoCompraMateriaPrima(
            data.PedidoCompraId,
            data.MateriaPrimaId,
            data.Quantidade,
            data.CustoUnitario);
        _context.PedidosCompraMateriasPrimas.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
        await RecalcularTotal(data.PedidoCompraId, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return item;
    }

    public async Task<PedidoCompraMateriaPrima> Update(
        int id,
        PedidoCompraMateriaPrimaDto data,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        var item = await _context.PedidosCompraMateriasPrimas
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O item de compra com o id {id} não foi localizado.");
        int pedidoAnteriorId = item.PedidoCompraId;
        await ValidarReferencias(data, id, cancellationToken);

        item.PedidoCompraId = data.PedidoCompraId;
        item.MateriaPrimaId = data.MateriaPrimaId;
        item.Quantidade = data.Quantidade;
        item.CustoUnitario = data.CustoUnitario;
        await _context.SaveChangesAsync(cancellationToken);
        await RecalcularTotal(pedidoAnteriorId, cancellationToken);
        if (pedidoAnteriorId != data.PedidoCompraId)
        {
            await RecalcularTotal(data.PedidoCompraId, cancellationToken);
        }
        await transaction.CommitAsync(cancellationToken);
        return item;
    }

    public async Task<PedidoCompraMateriaPrima> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        var item = await _context.PedidosCompraMateriasPrimas
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O item de compra com o id {id} não foi localizado.");
        await ValidarPedidoEditavel(item.PedidoCompraId, cancellationToken);

        int pedidoId = item.PedidoCompraId;
        _context.PedidosCompraMateriasPrimas.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
        await RecalcularTotal(pedidoId, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return item;
    }

    private async Task ValidarReferencias(
        PedidoCompraMateriaPrimaDto data,
        int? ignorarId,
        CancellationToken cancellationToken)
    {
        await ValidarPedidoEditavel(data.PedidoCompraId, cancellationToken);
        if (!await _context.MateriasPrimas.AnyAsync(x => x.Id == data.MateriaPrimaId, cancellationToken))
        {
            throw new KeyNotFoundException(
                $"A matéria-prima com o id {data.MateriaPrimaId} não foi localizada.");
        }

        bool duplicado = await _context.PedidosCompraMateriasPrimas.AnyAsync(
            x => x.PedidoCompraId == data.PedidoCompraId &&
                 x.MateriaPrimaId == data.MateriaPrimaId &&
                 (!ignorarId.HasValue || x.Id != ignorarId.Value),
            cancellationToken);
        if (duplicado)
        {
            throw new ConflitoNegocioException("A matéria-prima já foi incluída neste pedido de compra.");
        }
    }

    private async Task ValidarPedidoEditavel(int pedidoId, CancellationToken cancellationToken)
    {
        var status = await _context.PedidosCompra
            .Where(x => x.Id == pedidoId)
            .Select(x => (Status?)x.Status)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KeyNotFoundException($"O pedido de compra com o id {pedidoId} não foi localizado.");
        if (status != Status.Pendente)
        {
            throw new RegraNegocioException(
                "Os itens só podem ser alterados enquanto o pedido de compra estiver pendente.");
        }
    }

    private async Task RecalcularTotal(int pedidoId, CancellationToken cancellationToken)
    {
        decimal total = await _context.PedidosCompraMateriasPrimas
            .Where(x => x.PedidoCompraId == pedidoId)
            .SumAsync(x => x.Quantidade * x.CustoUnitario, cancellationToken);
        await _context.PedidosCompra
            .Where(x => x.Id == pedidoId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.ValorTotal, total), cancellationToken);
        await _context.ContasPagar
            .Where(x => x.PedidoCompraId == pedidoId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Valor, total), cancellationToken);
    }
}

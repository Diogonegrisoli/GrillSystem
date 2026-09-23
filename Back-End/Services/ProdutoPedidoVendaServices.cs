using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services;

public class ProdutoPedidoVendaServices
{
    private readonly AppDbContext _context;

    public ProdutoPedidoVendaServices(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<ProdutoPedidoVenda>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.ProdutosPedidosVenda
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<ProdutoPedidoVenda> GetId(
        int id,
        CancellationToken cancellationToken = default) =>
        await _context.ProdutosPedidosVenda
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException(
            $"O item de venda com o id {id} não foi localizado.");

    public async Task<ProdutoPedidoVenda> Create(
        ProdutoPedidoVendaDto data,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database
            .BeginTransactionAsync(cancellationToken);

        var produto = await _context.Produtos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == data.ProdutoId, cancellationToken)
            ?? throw new KeyNotFoundException($"O produto com o id {data.ProdutoId} não foi localizado.");
        await ValidarPedidoEditavel(data.PedidoVendaId, cancellationToken);
        await ValidarDuplicidade(data.PedidoVendaId, data.ProdutoId, null, cancellationToken);

        var item = new ProdutoPedidoVenda(
            data.Quantidade,
            produto.Preco,
            data.ProdutoId,
            data.PedidoVendaId);

        _context.ProdutosPedidosVenda.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
        await RecalcularTotal(data.PedidoVendaId, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return item;
    }

    public async Task<ProdutoPedidoVenda> Update(
        int id,
        ProdutoPedidoVendaDto data,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database
            .BeginTransactionAsync(cancellationToken);

        var item = await _context.ProdutosPedidosVenda
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O item de venda com o id {id} não foi localizado.");
        int pedidoAnteriorId = item.PedidoVendaId;
        var produto = await _context.Produtos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == data.ProdutoId, cancellationToken)
            ?? throw new KeyNotFoundException($"O produto com o id {data.ProdutoId} não foi localizado.");

        await ValidarPedidoEditavel(item.PedidoVendaId, cancellationToken);
        await ValidarPedidoEditavel(data.PedidoVendaId, cancellationToken);
        await ValidarDuplicidade(data.PedidoVendaId, data.ProdutoId, id, cancellationToken);

        item.Quantidade = data.Quantidade;
        item.PrecoUnitario = produto.Preco;
        item.ProdutoId = data.ProdutoId;
        item.PedidoVendaId = data.PedidoVendaId;
        await _context.SaveChangesAsync(cancellationToken);
        await RecalcularTotal(pedidoAnteriorId, cancellationToken);
        if (pedidoAnteriorId != data.PedidoVendaId)
        {
            await RecalcularTotal(data.PedidoVendaId, cancellationToken);
        }
        await transaction.CommitAsync(cancellationToken);
        return item;
    }

    public async Task<ProdutoPedidoVenda> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database
            .BeginTransactionAsync(cancellationToken);
        var item = await _context.ProdutosPedidosVenda
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O item de venda com o id {id} não foi localizado.");
        await ValidarPedidoEditavel(item.PedidoVendaId, cancellationToken);

        int pedidoId = item.PedidoVendaId;
        _context.ProdutosPedidosVenda.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
        await RecalcularTotal(pedidoId, cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return item;
    }

    private async Task ValidarPedidoEditavel(int pedidoId, CancellationToken cancellationToken)
    {
        var status = await _context.PedidosVenda
            .Where(x => x.Id == pedidoId)
            .Select(x => (StatusPedido?)x.Status)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KeyNotFoundException($"O pedido de venda com o id {pedidoId} não foi localizado.");

        if (status != StatusPedido.Pendente)
        {
            throw new RegraNegocioException(
                "Os itens só podem ser alterados enquanto o pedido de venda estiver pendente.");
        }
    }

    private async Task ValidarDuplicidade(
        int pedidoId,
        int produtoId,
        int? ignorarId,
        CancellationToken cancellationToken)
    {
        bool duplicado = await _context.ProdutosPedidosVenda.AnyAsync(
            x => x.PedidoVendaId == pedidoId && x.ProdutoId == produtoId &&
                 (!ignorarId.HasValue || x.Id != ignorarId.Value),
            cancellationToken);
        if (duplicado)
        {
            throw new ConflitoNegocioException("O produto já foi incluído neste pedido de venda.");
        }
    }

    private async Task RecalcularTotal(int pedidoId, CancellationToken cancellationToken)
    {
        decimal total = await _context.ProdutosPedidosVenda
            .Where(x => x.PedidoVendaId == pedidoId)
            .SumAsync(x => x.Quantidade * x.PrecoUnitario, cancellationToken);

        await _context.PedidosVenda
            .Where(x => x.Id == pedidoId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.ValorTotal, total), cancellationToken);
        await _context.ContasReceber
            .Where(x => x.PedidoVendaId == pedidoId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Valor, total), cancellationToken);
    }
}

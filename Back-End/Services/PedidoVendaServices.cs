using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using GrillSystem.Validacao;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Services;

public class PedidoVendaServices
{
    private readonly AppDbContext _context;

    public PedidoVendaServices(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<PedidoVenda>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.PedidosVenda
            .AsNoTracking()
            .OrderByDescending(x => x.DataPedido)
            .ThenByDescending(x => x.Id)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<PedidoVenda> GetId(
        int id,
        CancellationToken cancellationToken = default) =>
        await _context.PedidosVenda
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"O pedido de venda com o id {id} não foi localizado.");

    public async Task<PedidoVenda> Create(
        PedidoVendaDto data,
        CancellationToken cancellationToken = default)
    {
        ValidarDatas(data.DataPedido, data.DataEntrega);
        if (!await _context.Clientes.AnyAsync(x => x.Id == data.ClienteId, cancellationToken))
        {
            throw new KeyNotFoundException($"O cliente com o id {data.ClienteId} não foi localizado.");
        }

        var pedido = new PedidoVenda(data.DataPedido, data.DataEntrega, 0, data.ClienteId);
        _context.PedidosVenda.Add(pedido);
        await _context.SaveChangesAsync(cancellationToken);
        return pedido;
    }

    public async Task<PedidoVenda> Update(
        int id,
        PedidoVendaUpdateDto data,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        var pedido = await _context.PedidosVenda
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O pedido de venda com o id {id} não foi localizado.");

        StatusPedido novoStatus = data.Status
            ?? throw new ValidationException("O status do pedido deve ser informado.");
        ValidarTransicao(pedido.Status, novoStatus);
        ValidarDatas(data.DataPedido, data.DataEntrega);
        if (pedido.Status == StatusPedido.Pendente && novoStatus == StatusPedido.EmProducao)
        {
            await ValidarItensParaProducao(id, cancellationToken);
        }

        if (pedido.Status == StatusPedido.Pendente)
        {
            if (!await _context.Clientes.AnyAsync(x => x.Id == data.ClienteId, cancellationToken))
            {
                throw new KeyNotFoundException($"O cliente com o id {data.ClienteId} não foi localizado.");
            }
            pedido.DataPedido = data.DataPedido;
            pedido.DataEntrega = data.DataEntrega;
            pedido.ClienteId = data.ClienteId;
        }
        else if (pedido.DataPedido != data.DataPedido ||
                 pedido.DataEntrega != data.DataEntrega ||
                 pedido.ClienteId != data.ClienteId)
        {
            throw new RegraNegocioException(
                "Dados cadastrais do pedido só podem ser alterados enquanto ele estiver pendente.");
        }

        if (pedido.Status != StatusPedido.Enviado && novoStatus == StatusPedido.Enviado)
        {
            int pedidoReservado = await _context.PedidosVenda
                .Where(x => x.Id == id && x.Status == StatusPedido.EmProducao)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(x => x.Status, StatusPedido.Enviado),
                    cancellationToken);
            if (pedidoReservado == 0)
            {
                throw new ConflitoNegocioException(
                    "O pedido já foi enviado ou alterado por outra operação.");
            }
            await BaixarEstoqueProdutos(id, cancellationToken);
        }

        pedido.Status = novoStatus;
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return pedido;
    }

    public async Task<PedidoVenda> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        var pedido = await _context.PedidosVenda
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O pedido de venda com o id {id} não foi localizado.");
        if (pedido.Status != StatusPedido.Pendente)
        {
            throw new RegraNegocioException("Somente pedidos pendentes podem ser excluídos.");
        }
        if (await _context.ContasReceber.AnyAsync(x => x.PedidoVendaId == id, cancellationToken))
        {
            throw new ConflitoNegocioException("O pedido possui uma conta a receber e não pode ser excluído.");
        }

        _context.PedidosVenda.Remove(pedido);
        await _context.SaveChangesAsync(cancellationToken);
        return pedido;
    }

    private static void ValidarDatas(DateOnly dataPedido, DateOnly dataEntrega)
    {
        Validacoes.DataNaoFutura(dataPedido, "A data do pedido");
        Validacoes.PeriodoValido(dataPedido, dataEntrega, "data do pedido", "data de entrega");
    }

    private static void ValidarTransicao(StatusPedido atual, StatusPedido novo)
    {
        if (atual == novo)
        {
            return;
        }

        bool permitida = (atual, novo) switch
        {
            (StatusPedido.Pendente, StatusPedido.EmProducao) => true,
            (StatusPedido.Pendente, StatusPedido.Cancelado) => true,
            (StatusPedido.EmProducao, StatusPedido.Enviado) => true,
            (StatusPedido.EmProducao, StatusPedido.Cancelado) => true,
            (StatusPedido.Enviado, StatusPedido.Entregue) => true,
            _ => false
        };

        if (!permitida)
        {
            throw new RegraNegocioException($"Transição de {atual} para {novo} não permitida.");
        }
    }

    private async Task BaixarEstoqueProdutos(int pedidoId, CancellationToken cancellationToken)
    {
        var itens = await _context.ProdutosPedidosVenda
            .AsNoTracking()
            .Where(x => x.PedidoVendaId == pedidoId)
            .OrderBy(x => x.ProdutoId)
            .ToListAsync(cancellationToken);
        if (itens.Count == 0)
        {
            throw new RegraNegocioException("O pedido não possui produtos.");
        }
        if (itens.Any(x => x.Quantidade <= 0))
        {
            throw new RegraNegocioException(
                "O pedido possui produto sem quantidade válida. Revise os itens antes do envio.");
        }

        foreach (var item in itens)
        {
            int atualizados = await _context.Produtos
                .Where(x => x.Id == item.ProdutoId && x.Quantidade >= item.Quantidade)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(
                        x => x.Quantidade,
                        x => x.Quantidade - item.Quantidade),
                    cancellationToken);
            if (atualizados == 0)
            {
                throw new RegraNegocioException(
                    $"Estoque insuficiente para enviar o produto {item.ProdutoId}.");
            }
        }
    }

    private async Task ValidarItensParaProducao(
        int pedidoId,
        CancellationToken cancellationToken)
    {
        bool possuiItens = await _context.ProdutosPedidosVenda
            .AnyAsync(x => x.PedidoVendaId == pedidoId, cancellationToken);
        if (!possuiItens)
        {
            throw new RegraNegocioException(
                "Inclua ao menos um produto antes de enviar o pedido para produção.");
        }
    }
}

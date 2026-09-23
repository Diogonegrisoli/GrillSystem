using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using GrillSystem.Validacao;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Services;

public class PedidoCompraService
{
    private readonly AppDbContext _context;

    public PedidoCompraService(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<PedidoCompra>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.PedidosCompra
            .AsNoTracking()
            .OrderByDescending(x => x.DataPedido)
            .ThenByDescending(x => x.Id)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<PedidoCompra> GetId(
        int id,
        CancellationToken cancellationToken = default) =>
        await _context.PedidosCompra
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"O pedido de compra com o id {id} não foi localizado.");

    public async Task<PedidoCompra> Create(
        PedidoCompraDto data,
        CancellationToken cancellationToken = default)
    {
        ValidarDatas(data.DataPedido, data.DataEntrega);
        await ValidarReferencias(data.FornecedorId, data.FuncionarioId, cancellationToken);
        var pedido = new PedidoCompra(
            data.DataPedido,
            data.DataEntrega,
            0,
            data.FornecedorId,
            data.FuncionarioId);
        _context.PedidosCompra.Add(pedido);
        await _context.SaveChangesAsync(cancellationToken);
        return pedido;
    }

    public async Task<PedidoCompra> Update(
        int id,
        PedidoCompraUpdateDto data,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        var pedido = await _context.PedidosCompra
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O pedido de compra com o id {id} não foi localizado.");

        Status novoStatus = data.Status
            ?? throw new ValidationException("O status do pedido deve ser informado.");
        ValidarTransicao(pedido.Status, novoStatus);
        ValidarDatas(data.DataPedido, data.DataEntrega);
        if (pedido.Status == Status.Pendente && novoStatus == Status.Solicitado)
        {
            await ValidarItensParaSolicitacao(id, cancellationToken);
        }
        if (pedido.Status == Status.Pendente)
        {
            await ValidarReferencias(data.FornecedorId, data.FuncionarioId, cancellationToken);
            pedido.DataPedido = data.DataPedido;
            pedido.DataEntrega = data.DataEntrega;
            pedido.FornecedorId = data.FornecedorId;
            pedido.FuncionarioId = data.FuncionarioId;
        }
        else if (pedido.DataPedido != data.DataPedido ||
                 pedido.FornecedorId != data.FornecedorId ||
                 pedido.FuncionarioId != data.FuncionarioId)
        {
            throw new RegraNegocioException(
                "Dados cadastrais do pedido só podem ser alterados enquanto ele estiver pendente.");
        }

        if (pedido.Status != Status.Entregue && novoStatus == Status.Entregue)
        {
            int pedidoReservado = await _context.PedidosCompra
                .Where(x => x.Id == id && x.Status == Status.EmAndamento)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(x => x.Status, Status.Entregue),
                    cancellationToken);
            if (pedidoReservado == 0)
            {
                throw new ConflitoNegocioException(
                    "O pedido já foi recebido ou alterado por outra operação.");
            }
            await ReceberMateriais(pedido, data.DataEntrega, cancellationToken);
        }

        pedido.DataEntrega = data.DataEntrega;
        pedido.Status = novoStatus;
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return pedido;
    }

    public async Task<PedidoCompra> Delete(
        int id,
        CancellationToken cancellationToken = default)
    {
        var pedido = await _context.PedidosCompra
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"O pedido de compra com o id {id} não foi localizado.");
        if (pedido.Status != Status.Pendente)
        {
            throw new RegraNegocioException("Somente pedidos pendentes podem ser excluídos.");
        }
        if (await _context.ContasPagar.AnyAsync(x => x.PedidoCompraId == id, cancellationToken))
        {
            throw new ConflitoNegocioException("O pedido possui uma conta a pagar e não pode ser excluído.");
        }
        _context.PedidosCompra.Remove(pedido);
        await _context.SaveChangesAsync(cancellationToken);
        return pedido;
    }

    private static void ValidarDatas(DateOnly dataPedido, DateOnly? dataEntrega)
    {
        Validacoes.DataNaoFutura(dataPedido, "A data do pedido");
        Validacoes.PeriodoValido(dataPedido, dataEntrega, "data do pedido", "data de entrega");
    }

    private async Task ValidarReferencias(
        int fornecedorId,
        int funcionarioId,
        CancellationToken cancellationToken)
    {
        if (!await _context.Fornecedores.AnyAsync(x => x.Id == fornecedorId, cancellationToken))
        {
            throw new KeyNotFoundException($"O fornecedor com o id {fornecedorId} não foi localizado.");
        }
        if (!await _context.Funcionarios.AnyAsync(
                x => x.Id == funcionarioId && x.Status == StatusFuncionario.Ativo,
                cancellationToken))
        {
            throw new KeyNotFoundException(
                $"O funcionário com o id {funcionarioId} não existe ou está inativo.");
        }
    }

    private static void ValidarTransicao(Status atual, Status novo)
    {
        if (atual == novo)
        {
            return;
        }
        bool permitida = (atual, novo) switch
        {
            (Status.Pendente, Status.Solicitado) => true,
            (Status.Solicitado, Status.EmAndamento) => true,
            (Status.EmAndamento, Status.Entregue) => true,
            _ => false
        };
        if (!permitida)
        {
            throw new RegraNegocioException($"Transição de {atual} para {novo} não permitida.");
        }
    }

    private async Task ReceberMateriais(
        PedidoCompra pedido,
        DateOnly? dataEntrega,
        CancellationToken cancellationToken)
    {
        if (!dataEntrega.HasValue)
        {
            throw new RegraNegocioException("Informe a data de entrega para concluir o recebimento.");
        }
        Validacoes.DataNaoFutura(dataEntrega.Value, "A data de entrega");
        var itens = await _context.PedidosCompraMateriasPrimas
            .AsNoTracking()
            .Where(x => x.PedidoCompraId == pedido.Id)
            .OrderBy(x => x.MateriaPrimaId)
            .ToListAsync(cancellationToken);
        if (itens.Count == 0)
        {
            throw new RegraNegocioException("O pedido não possui matérias-primas.");
        }
        if (itens.Any(x => x.Quantidade <= 0 || x.CustoUnitario <= 0))
        {
            throw new RegraNegocioException(
                "O pedido possui item sem quantidade ou custo válidos. Revise os itens antes do recebimento.");
        }

        foreach (var item in itens)
        {
            int atualizados = await _context.MateriasPrimas
                .Where(x => x.Id == item.MateriaPrimaId)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(
                        x => x.Quantidade,
                        x => x.Quantidade + item.Quantidade),
                    cancellationToken);
            if (atualizados == 0)
            {
                throw new KeyNotFoundException(
                    $"A matéria-prima com o id {item.MateriaPrimaId} não foi localizada.");
            }

            _context.MovimentacoesEstoque.Add(new MovimentacaoEstoque(
                TipoMovimentacao.Entrada,
                item.Quantidade,
                item.CustoUnitario,
                dataEntrega.Value,
                $"Recebimento do pedido de compra #{pedido.Id}",
                item.MateriaPrimaId));
        }
    }

    private async Task ValidarItensParaSolicitacao(
        int pedidoId,
        CancellationToken cancellationToken)
    {
        var itens = await _context.PedidosCompraMateriasPrimas
            .AsNoTracking()
            .Where(x => x.PedidoCompraId == pedidoId)
            .Select(x => new { x.Quantidade, x.CustoUnitario })
            .ToListAsync(cancellationToken);
        if (itens.Count == 0)
        {
            throw new RegraNegocioException(
                "Inclua ao menos uma matéria-prima antes de solicitar a compra.");
        }
        if (itens.Any(x => x.Quantidade <= 0 || x.CustoUnitario <= 0))
        {
            throw new RegraNegocioException(
                "Todos os itens da compra devem possuir quantidade e custo unitário maiores que zero.");
        }
    }
}

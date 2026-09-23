using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using GrillSystem.Validacao;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Services;

public class ContaPagarService
{
    private readonly AppDbContext _context;

    public ContaPagarService(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<ContaPagar>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.ContasPagar.AsNoTracking()
            .OrderBy(x => x.Status).ThenBy(x => x.DataVencimento)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<ContaPagar> GetId(int id, CancellationToken cancellationToken = default) =>
        await _context.ContasPagar.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"A conta a pagar com o id {id} não foi localizada.");

    public async Task<ContaPagar> Create(
        ContaPagarDto data,
        CancellationToken cancellationToken = default)
    {
        DateTime emissao = Validacoes.DataEmissao(data.DataEmissao);
        TipoPagamento tipoPagamento = data.TipoPagamento
            ?? throw new ValidationException("O tipo de pagamento deve ser informado.");
        Validacoes.PeriodoValido(
            emissao,
            data.DataVencimento,
            data.DataPagamento,
            "data de emissão",
            "data de vencimento",
            "data de pagamento");
        if (data.DataPagamento.HasValue && data.DataPagamento.Value.Date > DateTime.Today)
        {
            throw new RegraNegocioException("A data de pagamento não pode ser futura.");
        }

        decimal total = await ObterTotalPedido(data.PedidoCompraId, cancellationToken);
        if (await _context.ContasPagar.AnyAsync(
                x => x.PedidoCompraId == data.PedidoCompraId,
                cancellationToken))
        {
            throw new ConflitoNegocioException("O pedido já possui uma conta a pagar.");
        }

        var conta = new ContaPagar(
            total,
            tipoPagamento,
            emissao,
            data.DataVencimento,
            data.DataPagamento,
            data.PedidoCompraId);
        _context.ContasPagar.Add(conta);
        await _context.SaveChangesAsync(cancellationToken);
        return conta;
    }

    public async Task<ContaPagar> Update(
        int id,
        ContaPagarUpdateDto data,
        CancellationToken cancellationToken = default)
    {
        var conta = await _context.ContasPagar
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A conta a pagar com o id {id} não foi localizada.");
        TipoPagamento tipoPagamento = data.TipoPagamento
            ?? throw new ValidationException("O tipo de pagamento deve ser informado.");
        Validacoes.PeriodoValido(
            conta.DataEmissao,
            data.DataVencimento,
            data.DataPagamento,
            "data de emissão",
            "data de vencimento",
            "data de pagamento");
        if (data.DataPagamento.HasValue && data.DataPagamento.Value.Date > DateTime.Today)
        {
            throw new RegraNegocioException("A data de pagamento não pode ser futura.");
        }
        if (await _context.ContasPagar.AnyAsync(
                x => x.PedidoCompraId == data.PedidoCompraId && x.Id != id,
                cancellationToken))
        {
            throw new ConflitoNegocioException("O pedido já possui outra conta a pagar.");
        }

        conta.Valor = await ObterTotalPedido(data.PedidoCompraId, cancellationToken);
        conta.TipoPagamento = tipoPagamento;
        conta.DataVencimento = data.DataVencimento;
        conta.DataPagamento = data.DataPagamento;
        conta.PedidoCompraId = data.PedidoCompraId;
        conta.Status = data.DataPagamento.HasValue ? StatusContaPagar.Pago : StatusContaPagar.Pendente;
        await _context.SaveChangesAsync(cancellationToken);
        return conta;
    }

    public async Task<ContaPagar> Delete(int id, CancellationToken cancellationToken = default)
    {
        var conta = await _context.ContasPagar
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A conta a pagar com o id {id} não foi localizada.");
        if (conta.Status == StatusContaPagar.Pago)
        {
            throw new RegraNegocioException("Uma conta paga não pode ser excluída.");
        }
        _context.ContasPagar.Remove(conta);
        await _context.SaveChangesAsync(cancellationToken);
        return conta;
    }

    private async Task<decimal> ObterTotalPedido(int pedidoId, CancellationToken cancellationToken)
    {
        decimal? total = await _context.PedidosCompra
            .Where(x => x.Id == pedidoId)
            .Select(x => (decimal?)x.ValorTotal)
            .FirstOrDefaultAsync(cancellationToken);
        if (!total.HasValue)
        {
            throw new KeyNotFoundException($"O pedido de compra com o id {pedidoId} não foi localizado.");
        }
        if (total.Value <= 0)
        {
            throw new RegraNegocioException("Inclua os itens do pedido antes de gerar a conta a pagar.");
        }
        return total.Value;
    }
}

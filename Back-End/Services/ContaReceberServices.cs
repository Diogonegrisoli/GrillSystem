using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using GrillSystem.Validacao;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Services;

public class ContaReceberServices
{
    private readonly AppDbContext _context;

    public ContaReceberServices(AppDbContext context) => _context = context;

    public Task<ResultadoPaginadoDto<ContaReceber>> ListAll(
        PaginacaoDto paginacao,
        CancellationToken cancellationToken = default) =>
        _context.ContasReceber.AsNoTracking()
            .OrderBy(x => x.StatusPagamento).ThenBy(x => x.DataVencimento)
            .PaginarAsync(paginacao, cancellationToken);

    public async Task<ContaReceber> GetId(int id, CancellationToken cancellationToken = default) =>
        await _context.ContasReceber.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new KeyNotFoundException($"A conta a receber com o id {id} não foi localizada.");

    public async Task<ContaReceber> Create(
        ContaReceberDto data,
        CancellationToken cancellationToken = default)
    {
        DateOnly emissao = DateOnly.FromDateTime(DateTime.Today);
        TipoPagamentoReceber tipoPagamento = data.TipoPagamento
            ?? throw new ValidationException("O tipo de pagamento deve ser informado.");
        ValidarDatas(emissao, data.DataVencimento, data.DataRecebimento);
        decimal total = await ObterTotalPedido(data.PedidoVendaId, cancellationToken);
        if (await _context.ContasReceber.AnyAsync(
                x => x.PedidoVendaId == data.PedidoVendaId,
                cancellationToken))
        {
            throw new ConflitoNegocioException("O pedido já possui uma conta a receber.");
        }

        var status = data.DataRecebimento.HasValue ? StatusPagamento.Pago : StatusPagamento.Pendente;
        var conta = new ContaReceber(
            total,
            data.DataVencimento,
            status,
            data.DataRecebimento,
            tipoPagamento,
            data.PedidoVendaId);
        _context.ContasReceber.Add(conta);
        await _context.SaveChangesAsync(cancellationToken);
        return conta;
    }

    public async Task<ContaReceber> Update(
        int id,
        ContaReceberUpdateDto data,
        CancellationToken cancellationToken = default)
    {
        var conta = await _context.ContasReceber
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A conta a receber com o id {id} não foi localizada.");
        TipoPagamentoReceber tipoPagamento = data.TipoPagamento
            ?? throw new ValidationException("O tipo de pagamento deve ser informado.");
        ValidarDatas(conta.DataEmissao, data.DataVencimento, data.DataRecebimento);
        if (await _context.ContasReceber.AnyAsync(
                x => x.PedidoVendaId == data.PedidoVendaId && x.Id != id,
                cancellationToken))
        {
            throw new ConflitoNegocioException("O pedido já possui outra conta a receber.");
        }

        conta.Valor = await ObterTotalPedido(data.PedidoVendaId, cancellationToken);
        conta.DataVencimento = data.DataVencimento;
        conta.DataRecebimento = data.DataRecebimento;
        conta.StatusPagamento = data.DataRecebimento.HasValue
            ? StatusPagamento.Pago
            : data.DataVencimento < DateOnly.FromDateTime(DateTime.Today)
                ? StatusPagamento.Atrasado
                : StatusPagamento.Pendente;
        conta.TipoPagamento = tipoPagamento;
        conta.PedidoVendaId = data.PedidoVendaId;
        await _context.SaveChangesAsync(cancellationToken);
        return conta;
    }

    public async Task<ContaReceber> Delete(int id, CancellationToken cancellationToken = default)
    {
        var conta = await _context.ContasReceber
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"A conta a receber com o id {id} não foi localizada.");
        if (conta.StatusPagamento == StatusPagamento.Pago)
        {
            throw new RegraNegocioException("Uma conta recebida não pode ser excluída.");
        }
        _context.ContasReceber.Remove(conta);
        await _context.SaveChangesAsync(cancellationToken);
        return conta;
    }

    private static void ValidarDatas(
        DateOnly emissao,
        DateOnly vencimento,
        DateOnly? recebimento)
    {
        Validacoes.PeriodoValido(emissao, vencimento, "data de emissão", "data de vencimento");
        if (recebimento.HasValue)
        {
            Validacoes.PeriodoValido(emissao, recebimento, "data de emissão", "data de recebimento");
            Validacoes.DataNaoFutura(recebimento.Value, "A data de recebimento");
        }
    }

    private async Task<decimal> ObterTotalPedido(int pedidoId, CancellationToken cancellationToken)
    {
        decimal? total = await _context.PedidosVenda
            .Where(x => x.Id == pedidoId)
            .Select(x => (decimal?)x.ValorTotal)
            .FirstOrDefaultAsync(cancellationToken);
        if (!total.HasValue)
        {
            throw new KeyNotFoundException($"O pedido de venda com o id {pedidoId} não foi localizado.");
        }
        if (total.Value <= 0)
        {
            throw new RegraNegocioException("Inclua os itens do pedido antes de gerar a conta a receber.");
        }
        return total.Value;
    }
}

using GrillSystem.Data;
using GrillSystem.Infrastructure;
using GrillSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services;

internal static class SincronizacaoFinanceira
{
    public static async Task AtualizarRecebimento(
        AppDbContext context, int pedidoId, decimal total, CancellationToken ct)
    {
        var conta = await context.ContasReceber.Include(x => x.Parcelas)
            .SingleOrDefaultAsync(x => x.PedidoVendaId == pedidoId, ct);
        if (conta is null) return;
        if (conta.Valor == total) return;
        if (conta.Parcelas.Any(x => x.Situacao != SituacaoParcela.Pendente))
            throw new RegraNegocioException("O pedido possui parcela já liquidada e não pode mudar de valor.");
        var parcelas = conta.Parcelas.OrderBy(x => x.NumeroParcela).ToList();
        if (parcelas.Count == 0)
            throw new ConflitoNegocioException("A conta não possui parcelas cadastradas.");
        var valores = Parcelamento.Gerar(total, parcelas.Count, parcelas[0].DataVencimento);
        for (int i = 0; i < parcelas.Count; i++) parcelas[i].ValorParcela = valores[i].Valor;
        conta.Valor = total;
        await context.SaveChangesAsync(ct);
    }

    public static async Task AtualizarPagamento(
        AppDbContext context, int pedidoId, decimal total, CancellationToken ct)
    {
        var conta = await context.ContasPagar.Include(x => x.Parcelas)
            .SingleOrDefaultAsync(x => x.PedidoCompraId == pedidoId, ct);
        if (conta is null) return;
        if (conta.Valor == total) return;
        if (conta.Parcelas.Any(x => x.Situacao != SituacaoParcela.Pendente))
            throw new RegraNegocioException("O pedido possui parcela já liquidada e não pode mudar de valor.");
        var parcelas = conta.Parcelas.OrderBy(x => x.NumeroParcela).ToList();
        if (parcelas.Count == 0)
            throw new ConflitoNegocioException("A conta não possui parcelas cadastradas.");
        var valores = Parcelamento.Gerar(total, parcelas.Count, parcelas[0].DataVencimento);
        for (int i = 0; i < parcelas.Count; i++) parcelas[i].ValorParcela = valores[i].Valor;
        conta.Valor = total;
        await context.SaveChangesAsync(ct);
    }
}

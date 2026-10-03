using GrillSystem.Infrastructure;

namespace GrillSystem.Services;

internal static class Parcelamento
{
    public static IReadOnlyList<(int Numero, decimal Valor, DateOnly Vencimento)> Gerar(
        decimal total, int quantidade, DateOnly primeiroVencimento)
    {
        if (total <= 0 || quantidade is < 1 or > 120 ||
            primeiroVencimento == default || total < quantidade * 0.01m)
        {
            throw new RegraNegocioException("Valor, quantidade de parcelas ou vencimento inválidos.");
        }

        decimal valorBase = Math.Round(total / quantidade, 2, MidpointRounding.AwayFromZero);
        var parcelas = new List<(int Numero, decimal Valor, DateOnly Vencimento)>();
        decimal acumulado = 0;
        for (int numero = 1; numero <= quantidade; numero++)
        {
            decimal valor = numero == quantidade ? total - acumulado : valorBase;
            if (valor <= 0)
            {
                throw new RegraNegocioException("A quantidade de parcelas é incompatível com o valor.");
            }
            parcelas.Add((numero, valor, primeiroVencimento.AddMonths(numero - 1)));
            acumulado += valor;
        }
        return parcelas;
    }
}

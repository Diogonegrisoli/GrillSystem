namespace GrillSystem.Models;

public class Caixa
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public TipoCaixa Tipo { get; set; }
    public SituacaoCaixa Situacao { get; set; } = SituacaoCaixa.Aberto;
    public decimal SaldoInicial { get; set; }
    public decimal SaldoFinal { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime? DataFechamento { get; set; }
}

public enum TipoCaixa { Fisico, Bancario }
public enum SituacaoCaixa { Aberto, Fechado }

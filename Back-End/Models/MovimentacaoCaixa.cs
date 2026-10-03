namespace GrillSystem.Models;

public class MovimentacaoCaixa
{
    public int Id { get; set; }
    public int CaixaId { get; set; }
    public Caixa Caixa { get; set; } = null!;
    public TipoMovimentacaoCaixa TipoMovimentacao { get; set; }
    public DateTime DataMovimentacao { get; set; }
    public decimal Valor { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public int? ParcelaPagarId { get; set; }
    public ContaPagarParcelada? ParcelaPagar { get; set; }
    public int? ParcelaReceberId { get; set; }
    public ContaReceberParcelada? ParcelaReceber { get; set; }
}

public enum TipoMovimentacaoCaixa { Entrada, Saida }

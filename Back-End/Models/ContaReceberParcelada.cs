using System.Text.Json.Serialization;

namespace GrillSystem.Models;

public class ContaReceberParcelada
{
    public int Id { get; set; }
    public int ContaReceberId { get; set; }
    [JsonIgnore]
    public ContaReceber ContaReceber { get; set; } = null!;
    public int NumeroParcela { get; set; }
    public decimal ValorParcela { get; set; }
    public DateOnly DataVencimento { get; set; }
    public DateOnly? DataRecebimento { get; set; }
    public SituacaoParcela Situacao { get; set; } = SituacaoParcela.Pendente;
    public ICollection<MovimentacaoCaixa> Movimentacoes { get; set; } = new List<MovimentacaoCaixa>();
}

using System.Text.Json.Serialization;

namespace GrillSystem.Models;

public class ContaPagarParcelada
{
    public int Id { get; set; }
    public int ContaPagarId { get; set; }
    [JsonIgnore]
    public ContaPagar ContaPagar { get; set; } = null!;
    public int NumeroParcela { get; set; }
    public decimal ValorParcela { get; set; }
    public DateOnly DataVencimento { get; set; }
    public DateOnly? DataPagamento { get; set; }
    public SituacaoParcela Situacao { get; set; } = SituacaoParcela.Pendente;
    public ICollection<MovimentacaoCaixa> Movimentacoes { get; set; } = new List<MovimentacaoCaixa>();
}

public enum SituacaoParcela { Pendente, Liquidada, Cancelada }

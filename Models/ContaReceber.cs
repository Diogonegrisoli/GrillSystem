namespace GrillSystem.Models
{
    public class ContaReceber
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public DateOnly DataVencimento { get; set; }
        public DateOnly? DataRecebimento { get; set; }
        public TipoPagamentoReceber TipoPagamento { get; set; }
        public StatusPagamento StatusPagamento { get; set; }
        public int PedidoVendaId { get; set; }
        public PedidoVenda PedidoVenda { get; set; }

        public ContaReceber() { }
        public ContaReceber(decimal valor, DateOnly dataVencimento, DateOnly dataRecebimento, TipoPagamentoReceber tipoPagamento, int pedidoVendaId)
        {
            Valor = valor;
            DataVencimento = dataVencimento;
            DataRecebimento = dataRecebimento;
            TipoPagamento = tipoPagamento;
            StatusPagamento = StatusPagamento.Pendente;
            PedidoVendaId = pedidoVendaId;
        }
    }

    public enum StatusPagamento
    {
        Pago,
        Pendente,
        Atrasado,
        Cancelado
    }
    public enum TipoPagamentoReceber
    {
        Pix,
        Dinheiro,
        Crédito,
        Débito,
        Boleto
    }
}

namespace GrillSystem.Models
{
    public class ContaReceber
    {
        public int Id { get; set; }
        public decimal Valor {  get; set; }
        public DateOnly DataVencimento { get; set; }
        public DateOnly? DataRecebimento {  get; set; }
        public TipoPagamento TipoPagamento { get; set; }
        public StatusPagamento StatusPagamento { get; set; }
        public int PedidoVendaId { get; set; }
        public PedidoVenda PedidoVenda { get; set; }
    }

    public enum StatusPagamento
    {
        Pago,
        Pendente,
        Atrasado,
        Cancelado
    }
    public enum TipoPagamento
    {
        Pix,
        Dinheiro,
        Crédito,
        Débito,
        Boleto
    }
}

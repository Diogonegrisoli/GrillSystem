namespace GrillSystem.Models
{
    public class ContaReceber
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public DateOnly DataVencimento { get; set; }
        private DateOnly? dataRecebimento;
        public DateOnly? DataRecebimento
        {
            get { return dataRecebimento; }
            set
            {
                if (value.HasValue && value < DataEmissao)
                {
                    throw new ArgumentException("A data do recebimento não pode ser menor que a data da emissão!");
                }
                dataRecebimento = value;
            }
        }
        public DateOnly DataEmissao { get; set; }
        public TipoPagamentoReceber TipoPagamento { get; set; }
        public StatusPagamento StatusPagamento { get; set; }
        public int PedidoVendaId { get; set; }
        public PedidoVenda PedidoVenda { get; set; } = null!;

        public ContaReceber() { }
        public ContaReceber(decimal valor, DateOnly dataVencimento, StatusPagamento statusPagamento, DateOnly? dataRecebimento, TipoPagamentoReceber tipoPagamento, int pedidoVendaId)
        {
            Valor = valor;
            DataVencimento = dataVencimento;
            DataEmissao = DateOnly.FromDateTime(DateTime.Today);
            DataRecebimento = dataRecebimento;
            TipoPagamento = tipoPagamento;
            StatusPagamento = statusPagamento;
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
        Credito,
        Debito,
        Boleto
    }
}

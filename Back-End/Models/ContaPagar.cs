using System.Diagnostics.Contracts;

namespace GrillSystem.Models
{
    public class ContaPagar
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public TipoPagamento TipoPagamento { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public StatusContaPagar Status { get; set; }
        public int PedidoCompraId {  get; set; }
        public PedidoCompra PedidoCompra { get; set; }

        public ContaPagar(){ }

        public ContaPagar(decimal valor, TipoPagamento tipoPagamento, DateTime dataEmissao, DateTime dataVencimento, DateTime? dataPagamento, int pedidoCompraId)
        {
            Valor = valor;
            TipoPagamento = tipoPagamento;
            DataEmissao = dataEmissao;
            DataVencimento = dataVencimento;
            DataPagamento = dataPagamento;
            PedidoCompraId = pedidoCompraId;
            Status = dataPagamento.HasValue ? StatusContaPagar.Pago : StatusContaPagar.Pendente;
        }
    }

    public enum TipoPagamento
    {
        Debito,
        Credito,
        Pix,
        Dinheiro
    }

    public enum StatusContaPagar
    {
        Pago,
        Pendente,
        Cancelado
    }
}

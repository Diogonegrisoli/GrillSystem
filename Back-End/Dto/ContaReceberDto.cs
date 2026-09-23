using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ContaReceberDto
    {
        [Required(ErrorMessage = "É necessário informar a data de vencimento!")]
        public DateOnly DataVencimento { get; set; }

        public DateOnly? DataRecebimento { get; set; }

        [Required(ErrorMessage = "É necessário informar o tipo de pagamento!")]
        [EnumDataType(typeof(TipoPagamentoReceber))]
        public TipoPagamentoReceber? TipoPagamento { get; set; }

        [Required(ErrorMessage = "É necessário informar o pedido da venda!")]
        [Range(1, int.MaxValue)]
        public int PedidoVendaId { get; set; }
    }

    public class ContaReceberUpdateDto
    {
        [Required(ErrorMessage = "É necessário informar a data de vencimento!")]
        public DateOnly DataVencimento { get; set; }
        public DateOnly? DataRecebimento { get; set; }
        [Required(ErrorMessage = "É necessário informar o tipo de pagamento!")]
        [EnumDataType(typeof(TipoPagamentoReceber))]
        public TipoPagamentoReceber? TipoPagamento { get; set; }
        [Required(ErrorMessage = "É necessário informar o pedido da venda!")]
        [Range(1, int.MaxValue)]
        public int PedidoVendaId { get; set; }
    }
}

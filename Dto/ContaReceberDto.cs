using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ContaReceberDto
    {
        [Required(ErrorMessage = "É necessário informar o valor!")]
        [Range(1, double.MaxValue, ErrorMessage = "O valor deve ser informado!")]
        public decimal Valor { get; set; }
        [Required(ErrorMessage = "É necessário informar a data de vencimento!")]
        public DateOnly DataVencimento { get; set; }
        public DateOnly? DataRecebimento { get; set; }
        [Required(ErrorMessage = "É necessário informar o tipo de pagamento!")]
        public TipoPagamentoReceber TipoPagamento { get; set; }
        [Required(ErrorMessage = "É necessário informar o status do pagamento")]
        public StatusPagamento StatusPagamento { get; set; }
        [Required(ErrorMessage = "É necessário informar o pedido da venda!")]
        public int Fk_PedidoVendaId { get; set; }
    }
}

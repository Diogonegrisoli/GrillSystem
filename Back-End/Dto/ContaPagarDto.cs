using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ContaPagarDto
    {
        [EnumDataType(typeof(TipoPagamento))]
        [Required(ErrorMessage = "O tipo de pagamento deve ser informado!")]
        public TipoPagamento? TipoPagamento { get; set; }
        [Required(ErrorMessage = "A data da emissão deve ser informada!")]
        public DateTime DataEmissao { get; set; }
        public DateTime? DataPagamento { get; set; }
        [Required(ErrorMessage = "A data do vencimento deve ser informada!")]
        public DateTime DataVencimento { get; set; }
        [Required(ErrorMessage = "O pedido de compra deve ser informado!")]
        [Range(1, int.MaxValue)]
        public int PedidoCompraId { get; set; }
    }

    public class ContaPagarUpdateDto
    {
        [EnumDataType(typeof(TipoPagamento))]
        [Required(ErrorMessage = "O tipo de pagamento deve ser informado!")]
        public TipoPagamento? TipoPagamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        [Required(ErrorMessage = "A data do vencimento deve ser informada!")]
        public DateTime DataVencimento { get; set; }
        [Required(ErrorMessage = "O pedido de compra deve ser informado!")]
        [Range(1, int.MaxValue)]
        public int PedidoCompraId { get; set; }
    }
}

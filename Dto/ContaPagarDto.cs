using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ContaPagarDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor não pode ser menor ou igual a zero!")]
        public decimal Valor { get; set; }
        public TipoPagamento TipoPagamento { get; set; }
        [Required(ErrorMessage = "A data da emissão deve ser informada!")]
        public DateTime DataEmissao { get; set; }
        public DateTime? DataPagamento { get; set; }
        [Required(ErrorMessage = "A data do vencimento deve ser informada!")]
        public DateTime DataVencimento { get; set; }
        [Required(ErrorMessage = "O pedido de compra deve ser informado!")]
        public int PedidoCompraId { get; set; }
    }

    public class ContaPagarUpdateDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor não pode ser menor ou igual a zero!")]
        public decimal Valor { get; set; }
        public TipoPagamento TipoPagamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        [Required(ErrorMessage = "A data do vencimento deve ser informada!")]
        public DateTime DataVencimento { get; set; }
        [Required(ErrorMessage = "O pedido de compra deve ser informado!")]
        public int PedidoCompraId { get; set; }
    }
}

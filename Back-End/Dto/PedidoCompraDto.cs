using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class PedidoCompraDto
    {
        [Required(ErrorMessage = "A data deve ser informada!")]
        public DateOnly DataPedido { get; set; }

        public DateOnly? DataEntrega { get; set; }

        [Required(ErrorMessage = "O fornecedor deve ser informado!")]
        [Range(1, int.MaxValue)]
        public int FornecedorId { get; set; }

        [Required(ErrorMessage = "O funcionário deve ser informado!")]
        [Range(1, int.MaxValue)]
        public int FuncionarioId { get; set; }
        [EnumDataType(typeof(TipoPagamento))]
        public TipoPagamento? FormaPagamento { get; set; }
        [Range(1, 120)]
        public int Parcelas { get; set; } = 1;
    }

    public class PedidoCompraUpdateDto
    {
        [Required(ErrorMessage = "A data deve ser informada!")]
        public DateOnly DataPedido { get; set; }

        public DateOnly? DataEntrega { get; set; }

        [Required(ErrorMessage = "O status do pedido deve ser informado!")]
        [EnumDataType(typeof(Status))]
        public Status? Status { get; set; }

        [Required(ErrorMessage = "O fornecedor deve ser informado!")]
        [Range(1, int.MaxValue)]
        public int FornecedorId { get; set; }

        [Required(ErrorMessage = "O funcionário deve ser informado!")]
        [Range(1, int.MaxValue)]
        public int FuncionarioId { get; set; }
        [EnumDataType(typeof(TipoPagamento))]
        public TipoPagamento? FormaPagamento { get; set; }
        [Range(1, 120)]
        public int Parcelas { get; set; } = 1;
    }
}

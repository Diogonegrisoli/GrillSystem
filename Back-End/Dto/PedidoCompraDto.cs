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
    }
}

using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class PedidoCompraDto
    {
        [Required(ErrorMessage = "A data deve ser informada!")]
        public DateOnly DataPedido { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor total não pode ser menor ou igual a zero!")]
        public decimal ValorTotal { get; set; }
        [Required(ErrorMessage = "O fornecedor deve ser informado!")]
        public int FornecedorId { get; set; }
        [Required(ErrorMessage = "O funcionário deve ser informado!")]
        public int FuncionarioId { get; set; }
    }
}

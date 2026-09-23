using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ProdutoPedidoVendaDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Deve haver no mínimo 1 produto!")]
        public int Quantidade { get; set; }
        [Required(ErrorMessage = "O produto deve ser informado!")]
        [Range(1, int.MaxValue)]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "O pedido da venda deve ser informado!")]
        [Range(1, int.MaxValue)]
        public int PedidoVendaId { get; set; }
    }
}

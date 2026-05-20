using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ProdutoPedidoVendaDto
    {
        [Required(ErrorMessage = "O produto deve ser informado!")]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "O pedido da venda deve ser informado!")]
        public int PedidoVendaId { get; set; }
    }
}

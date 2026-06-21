using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ProdutoPedidoVendaDto
    {
        [Required(ErrorMessage = "A quantidade deve ser informada!")]
        [MinLength(1, ErrorMessage =("Deve ser ter no mímo 1 produto!"))]
        public int Quantidade {  get; set; }
        [Required(ErrorMessage = "O produto deve ser informado!")]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "O pedido da venda deve ser informado!")]
        public int PedidoVendaId { get; set; }
    }
}

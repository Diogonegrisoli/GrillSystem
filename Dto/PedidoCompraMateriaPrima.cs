using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class PedidoCompraMateriaPrima
    {
        [Required(ErrorMessage = "O pedido de compra deve ser informado!")]
        public int PedidoCompraId { get; set; }
        [Required(ErrorMessage = "A matéria-prima deve ser informada!")]
        public int MateriaPrimaId { get; set; }
    }
}

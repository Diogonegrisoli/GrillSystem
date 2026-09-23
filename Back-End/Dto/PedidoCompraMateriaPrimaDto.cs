using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class PedidoCompraMateriaPrimaDto
    {
        [Required(ErrorMessage = "O pedido de compra deve ser informado!")]
        [Range(1, int.MaxValue)]
        public int PedidoCompraId { get; set; }
        [Required(ErrorMessage = "A matéria-prima deve ser informada!")]
        [Range(1, int.MaxValue)]
        public int MateriaPrimaId { get; set; }
        [Range(0.001, double.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero!")]
        public decimal Quantidade { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "O custo unitário deve ser maior que zero!")]
        public decimal CustoUnitario { get; set; }
    }
}

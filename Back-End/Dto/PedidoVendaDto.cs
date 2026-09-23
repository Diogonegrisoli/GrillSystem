using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class PedidoVendaDto
    {
        [Required(ErrorMessage = "É necessário informar a data do pedido!")]
        public DateOnly DataPedido { get; set; }
        [Required(ErrorMessage = "É necessário inoformar a data da entrega!")]
        public DateOnly DataEntrega { get; set; }
        [Required(ErrorMessage = "É necessário informar o cliente!")]
        [Range(1, int.MaxValue)]
        public int ClienteId { get; set; }
    }


    public class PedidoVendaUpdateDto
    {
        [Required(ErrorMessage = "É necessário informar a data do pedido!")]
        public DateOnly DataPedido { get; set; }
        [Required(ErrorMessage = "É necessário inoformar a data da entrega!")]
        public DateOnly DataEntrega { get; set; }
        [Required(ErrorMessage = "O status do pedido deve ser informado!")]
        [EnumDataType(typeof(StatusPedido))]
        public StatusPedido? Status { get; set; }
        [Required(ErrorMessage = "É necessário informar o cliente!")]
        [Range(1, int.MaxValue)]
        public int ClienteId { get; set; }
    }
}

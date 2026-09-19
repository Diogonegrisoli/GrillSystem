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
        [Required(ErrorMessage = "É necessário informar o valor total!")]
        public decimal ValorTotal { get; set; }
        [Required(ErrorMessage = "É necessário informar o cliente!")]
        public int ClienteId { get; set; }
    }


    public class PedidoVendaUpdateDto
    {
        [Required(ErrorMessage = "É necessário informar a data do pedido!")]
        public DateOnly DataPedido { get; set; }
        [Required(ErrorMessage = "É necessário inoformar a data da entrega!")]
        public DateOnly DataEntrega { get; set; }
        [Required(ErrorMessage = "É necessário informar o valor total!")]
        public StatusPedido Status { get; set; }
        public decimal ValorTotal { get; set; }
        [Required(ErrorMessage = "É necessário informar o cliente!")]
        public int ClienteId { get; set; }
    }
}

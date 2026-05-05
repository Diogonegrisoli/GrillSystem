namespace GrillSystem.Models
{
    public class PedidoVenda
    {
        public int Id { get; set; }
        public DateOnly DataPedido { get; set; }
        public DateOnly DataEntrega { get; set; }
        public StatusPedido Status { get; set; }
        public decimal ValorTotal { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public PedidoVenda() { }

        public PedidoVenda(DateOnly dataPedido, DateOnly dataEntrega, decimal valorTotal, int clienteId)
        {
            DataPedido = dataPedido;
            DataEntrega = dataEntrega;
            Status = StatusPedido.Pendente;
            ValorTotal = valorTotal;
            ClienteId = clienteId;
        }
    }

    public enum StatusPedido
    {
        Pendente,
        EmProdução,
        Enviado,
        Entregue,
        Cancelado
    }
}

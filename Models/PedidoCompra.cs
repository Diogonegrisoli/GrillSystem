namespace GrillSystem.Models
{
    public class PedidoCompra
    {
        public int Id {  get; set; }
        public DateOnly DataPedido { get; set; }
        public DateOnly? DataEntrega { get; set; }
        public Status Status { get; set; }
        public decimal ValorTotal { get; set; }
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }

        public PedidoCompra() { }

        public PedidoCompra(DateOnly dataPedido, DateOnly dataEntrega, decimal valorTotal, int fornecedorId, int funcionarioId)
        {
            DataPedido = dataPedido;
            DataEntrega = dataEntrega;
            Status = Status.Pendente;
            ValorTotal = valorTotal;
            FornecedorId = fornecedorId;
            FuncionarioId = funcionarioId;
        }
    }

    public enum Status
    {
        Entregue,
        EmAndamento,
        Solicitado,
        Pendente
    }
}

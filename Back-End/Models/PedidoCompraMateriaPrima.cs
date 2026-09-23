namespace GrillSystem.Models
{
    public class PedidoCompraMateriaPrima
    {
        public int Id { get; set; }
        public int PedidoCompraId { get; set; }
        public PedidoCompra PedidoCompra { get; set; } = null!;
        public int MateriaPrimaId { get; set; }
        public MateriaPrima MateriaPrima { get; set; } = null!;
        public decimal Quantidade { get; set; }
        public decimal CustoUnitario { get; set; }

        public PedidoCompraMateriaPrima() { }

        public PedidoCompraMateriaPrima(
            int pedidoCompraId,
            int materiaPrimaId,
            decimal quantidade,
            decimal custoUnitario)
        {
            PedidoCompraId = pedidoCompraId;
            MateriaPrimaId = materiaPrimaId;
            Quantidade = quantidade;
            CustoUnitario = custoUnitario;
        }
    }
}

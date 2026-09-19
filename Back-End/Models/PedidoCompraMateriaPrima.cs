namespace GrillSystem.Models
{
    public class PedidoCompraMateriaPrima
    {
        public int Id {  get; set; }
        public int PedidoCompraId { get; set; }
        public PedidoCompra PedidoCompra { get; set; }
        public int MateriaPrimaId { get; set; }
        public MateriaPrima MateriaPrima { get; set;}

        public PedidoCompraMateriaPrima() { }

        public PedidoCompraMateriaPrima(int pedidoCompraId, int mmateriaPrimaId)
        {
            PedidoCompraId = pedidoCompraId;
            MateriaPrimaId = mmateriaPrimaId;
        }
    }
}

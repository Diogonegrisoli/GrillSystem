namespace GrillSystem.Models
{
    public class ProdutoPedidoVenda
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int PedidoVendaId { get; set; }
        public PedidoVenda PedidoVenda { get; set; }


        public ProdutoPedidoVenda() { }

        public ProdutoPedidoVenda(int produtoId, int pedidoVendaId)
        {
            ProdutoId = produtoId;
            PedidoVendaId = pedidoVendaId;
        }
    }
}

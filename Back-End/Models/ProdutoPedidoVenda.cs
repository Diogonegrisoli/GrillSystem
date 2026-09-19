namespace GrillSystem.Models
{
    public class ProdutoPedidoVenda
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int PedidoVendaId { get; set; }
        public PedidoVenda PedidoVenda { get; set; }


        public ProdutoPedidoVenda() { }

        public ProdutoPedidoVenda(int quantidade, int produtoId, int pedidoVendaId)
        {
            Quantidade = quantidade;
            ProdutoId = produtoId;
            PedidoVendaId = pedidoVendaId;
        }
    }
}

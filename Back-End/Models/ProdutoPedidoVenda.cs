namespace GrillSystem.Models
{
    public class ProdutoPedidoVenda
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;
        public int PedidoVendaId { get; set; }
        public PedidoVenda PedidoVenda { get; set; } = null!;


        public ProdutoPedidoVenda() { }

        public ProdutoPedidoVenda(int quantidade, decimal precoUnitario, int produtoId, int pedidoVendaId)
        {
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
            ProdutoId = produtoId;
            PedidoVendaId = pedidoVendaId;
        }
    }
}

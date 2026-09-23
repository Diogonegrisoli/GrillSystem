namespace GrillSystem.Models
{
    public class ProdutoOrdemProducao
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;
        public int OrdemProducaoId { get; set; }
        public OrdemProducao OrdemProducao { get; set; } = null!;

        public ProdutoOrdemProducao() { }

        public ProdutoOrdemProducao(int produtoId, int ordemProducaoId)
        {
            ProdutoId = produtoId;
            OrdemProducaoId = ordemProducaoId;
        }
    }
}

namespace GrillSystem.Models
{
    public class ProdutoOrdemProducao
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int OrdemProducaoId { get; set; }
        public OrdemProducao OrdemProducao { get; set; }

        public ProdutoOrdemProducao() { }

        public ProdutoOrdemProducao(int produtoId, int ordemProducaoId)
        {
            ProdutoId = produtoId;
            OrdemProducaoId = ordemProducaoId;
        }
    }
}

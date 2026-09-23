namespace GrillSystem.Models
{
    public class ProdutoMateriaPrima
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int MateriaPrimaId { get; set; }
        public MateriaPrima MateriaPrima { get; set; }
        public decimal QuantidadeNecessaria { get; set; }
        public ProdutoMateriaPrima() { }

        public ProdutoMateriaPrima(int produtoId, int materiaPrimaId)
        {
            ProdutoId = produtoId;
            MateriaPrimaId = materiaPrimaId;
        }
    }
}

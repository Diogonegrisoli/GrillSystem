namespace GrillSystem.Models
{
    public class ProdutoMateriaPrima
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;
        public int MateriaPrimaId { get; set; }
        public MateriaPrima MateriaPrima { get; set; } = null!;
        public decimal QuantidadeNecessaria { get; set; }
        public ProdutoMateriaPrima() { }

        public ProdutoMateriaPrima(int produtoId, int materiaPrimaId, decimal quantidadeNecessaria)
        {
            ProdutoId = produtoId;
            MateriaPrimaId = materiaPrimaId;
            QuantidadeNecessaria = quantidadeNecessaria;
        }
    }
}

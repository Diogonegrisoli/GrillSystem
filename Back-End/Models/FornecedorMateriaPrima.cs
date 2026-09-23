namespace GrillSystem.Models
{
    public class FornecedorMateriaPrima
    {
        public int Id { get; set; }
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; } = null!;
        public int MateriaPrimaId { get; set; }
        public MateriaPrima MateriaPrima { get; set; } = null!;

        public FornecedorMateriaPrima() { }

        public FornecedorMateriaPrima(int fornecedorId, int materiaPrimaId)
        {
            FornecedorId = fornecedorId;
            MateriaPrimaId = materiaPrimaId;
        }
    }
}

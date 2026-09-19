using GrillSystem.Models;

namespace GrillSystem.Dto
{
    public class MateriaPrimaDto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal Quantidade { get; set; }
        public decimal QuantidadeMinima { get; set; }
        public UnidadeMedida UnidadeMedida { get; set; }
    }
}

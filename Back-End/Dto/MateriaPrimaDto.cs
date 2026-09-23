using GrillSystem.Models;

using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class MateriaPrimaDto
    {
        [Required, MaxLength(50)]
        public string Codigo { get; set; } = string.Empty;
        [Required, MaxLength(250)]
        public string Descricao { get; set; } = string.Empty;
        [Range(0, double.MaxValue)]
        public decimal QuantidadeMinima { get; set; }
        [EnumDataType(typeof(UnidadeMedida))]
        public UnidadeMedida UnidadeMedida { get; set; }
    }
}

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
        [EnumDataType(typeof(SituacaoCadastro))]
        public SituacaoCadastro Situacao { get; set; } = SituacaoCadastro.Ativo;
        [MaxLength(1000)]
        public string Observacoes { get; set; } = string.Empty;
    }
}

using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class MovimentacaoEstoqueDto
    {
        [Required(ErrorMessage = "O tipo da movimentação deve ser informado!")]
        [EnumDataType(typeof(TipoMovimentacao))]
        public TipoMovimentacao? Tipo { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "A quantidade não pode ser negativa!")]
        public decimal Quantidade { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "O custo unitário não pode menor ou igual a zero!")]
        public decimal CustoUnitario { get; set; }
        [Required(ErrorMessage = "A data deve ser informada!")]
        public DateOnly Data { get; set; }
        [Required(ErrorMessage = "Deve ser informado uma referência!")]
        [MaxLength(200)]
        public string Referencia { get; set; } = string.Empty;
        [Required(ErrorMessage = "A matéria-prima deve ser informada!")]
        [Range(1, int.MaxValue)]
        public int MateriaPrimaId { get; set; }
    }
}

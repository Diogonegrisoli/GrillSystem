using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class MovimentacaoEstoqueDto
    {
        public TipoMovimentacao Tipo { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "A quantidade não pode ser negativa!")]
        public decimal Quantidade { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "O custo unitário não pode menor ou igual a zero!")]
        public decimal CustoUnitario { get; set; }
        [Required(ErrorMessage = "A data deve ser informada!")]
        public DateOnly Data { get; set; }
        [Required(ErrorMessage = "Deve ser informado uma referência!")]
        public string Referencia { get; set; }
        [Required(ErrorMessage = "A matéria-prima deve ser informada!")]
        public int MateriaPrimaId { get; set; }
    }
}

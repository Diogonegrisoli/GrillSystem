using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ProdutoMateriaPrimaDto
    {
        [Required(ErrorMessage = "O produto deve ser informado!")]
        [Range(1, int.MaxValue)]
        public int ProdutoId { get; set; }
        [Range(1, int.MaxValue)]
        [Required(ErrorMessage = "A matéria-prima deve ser informada!")]
        public int MateriaPrimaId { get; set; }
        [Required(ErrorMessage = "A quantidade necessária deve ser informada!")]
        [Range(0.001, double.MaxValue, ErrorMessage = "A quantidade necessária deve ser maior que zero!")]
        public decimal QuantidadeNecessaria { get; set; }
    }
}

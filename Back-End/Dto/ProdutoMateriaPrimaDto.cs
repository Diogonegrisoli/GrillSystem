using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ProdutoMateriaPrimaDto
    {
        [Required(ErrorMessage = "O produto deve ser informado!")]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "A matéria-prima deve ser informada!")]
        public int MateriaPrimaId { get; set; }
    }
}

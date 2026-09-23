using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class FornecedorMateriaPrimaDto
    {
        [Required(ErrorMessage = "O fornecedor deve ser informado!")]
        [Range(1, int.MaxValue)]
        public int FornecedorId { get; set; }
        [Required(ErrorMessage = "A matéria-prima deve ser informada!")]
        [Range(1, int.MaxValue)]
        public int MateriaPrimaId { get; set; }
    }
}

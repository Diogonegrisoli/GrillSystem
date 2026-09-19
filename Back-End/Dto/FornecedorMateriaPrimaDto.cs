using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class FornecedorMateriaPrimaDto
    {
        [Required(ErrorMessage = "O fornecedor deve ser informado!")]
        public int FornecedorId { get; set; }
        [Required(ErrorMessage = "A matéria-prima deve ser informada!")]
        public int MateriaPrimaId { get; set; }
    }
}

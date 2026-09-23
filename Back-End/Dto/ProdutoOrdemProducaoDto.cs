using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ProdutoOrdemProducaoDto
    {
        [Required(ErrorMessage = "O produto deve ser informado!")]
        [Range(1, int.MaxValue)]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "A ordem de produção deve ser informada!")]
        [Range(1, int.MaxValue)]
        public int OrdemProducaoId { get; set; }
    }
}

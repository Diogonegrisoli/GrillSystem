using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ProdutoOrdemProducaoDto
    {
        [Required(ErrorMessage = "O produto deve ser informado!")]
        public int Fk_ProdutoId { get; set; }
        [Required(ErrorMessage = "A ordem de produção deve ser informada!")]
        public int FK_OrdemProducaoId { get; set; }
    }
}

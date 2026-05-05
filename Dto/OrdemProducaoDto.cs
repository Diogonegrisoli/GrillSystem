using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class OrdemProducaoDto
    {
        [Range(1,int.MaxValue,ErrorMessage = "A quantidade deve ser informada!")]
        public int Quantidade { get; set; }
        [Required(ErrorMessage = "A data inicial deve ser informada!")]
        public DateOnly DataInicio { get; set; }
        public DateOnly? DataFim { get; set; }
        [Required(ErrorMessage = "O tipo da ordem de produção deve ser informada!")]
        public TipoOrdem Tipo { get; set; }
    }
} 

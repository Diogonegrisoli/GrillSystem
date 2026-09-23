using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class CategoriaFinanceiraDto
    {
        [Required(ErrorMessage = "O nome deve ser informado!")]
        [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 letras!")]
        [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 letras!")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "O tipo é obrigatório!")]
        [EnumDataType(typeof(TipoCategoria), ErrorMessage = "Tipo inválido!")]
        public TipoCategoria? Tipo { get; set; }
    }
}

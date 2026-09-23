using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class FuncionarioDto
    {
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres!")]
        [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres!")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "O CPF é obrigatório!")]
        [MinLength(11, ErrorMessage = "O CPF é inválido!")]
        public string Cpf { get; set; } = string.Empty;
    }
}

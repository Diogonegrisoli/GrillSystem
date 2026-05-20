using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class UsuarioDto
    {
        [Required(ErrorMessage = "O email é obrigatório!")]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        [MaxLength(150, ErrorMessage = "E-mail muito longo!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória!")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres!")]
        public string SenhaHash { get; set; }
        [Required(ErrorMessage = "O usuário deve ser informado!")]
        public int FuncionarioId { get; set; }
    }
}

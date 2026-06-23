using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class UsuarioDto
    {
        [Required(ErrorMessage = "O email é obrigatório!")]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        [MaxLength(150, ErrorMessage = "E-mail muito longo!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória!")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres!")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O usuário deve ser informado!")]
        public int FuncionarioId { get; set; }
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "O email é obrigatório!")]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória!")]
        public string Senha { get; set; } = string.Empty;
    }

    public class TokenDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiraEm { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class FornecedorDto
    {
        [Required(ErrorMessage = "A razão social deve ser informada!")]
        [MaxLength(150, ErrorMessage = "A razão social deve ter no máximo 150 caracteres!")]
        public string RazaoSocial { get; set; }
        [Required(ErrorMessage = "O nome fantasia deve ser informado!")]
        [MaxLength(200, ErrorMessage = "O nome fantasia deve ter no máximo 200 caracteres!")]
        public string NomeFantasia { get; set; }
        [Required(ErrorMessage = "O CNPJ deve ser informado!")]
        [MaxLength(14, ErrorMessage = "CNPJ inválido!")]
        [MinLength(14, ErrorMessage = "CNPJ inválido!")]
        public string Cnpj { get; set; }
        [Required(ErrorMessage = "O e-mail deve ser informado!")]
        [MaxLength(250, ErrorMessage = "O e-mail deve ter no máximo 250 caracteres!")]
        [EmailAddress(ErrorMessage = "E-mail inválido!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O endereço deve ser informado!")]
        [MaxLength(300, ErrorMessage = "O endereço atingiu o número máximo de 300 caracteres!")]
        public string Endereco { get; set; }
    }
}

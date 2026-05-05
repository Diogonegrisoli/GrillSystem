using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ClienteDto
    {
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O CPF/CNPJ é obrigatório!")]
        [MinLength(11, ErrorMessage = "O CPF/CNPJ inválido!")]
        [MaxLength(14, ErrorMessage = "O CPF/CNPJ inválido!")]
        public string CpfCnpj { get; set; }                  
        [Required(ErrorMessage = "Tipo do cliente é obrigatório!")]
        public TipoCliente Tipo { get; set; }
        [Required(ErrorMessage = "O telefone é obrigatório!")]
        [MinLength(10, ErrorMessage = "Telefone inválido!")]
        [MaxLength(15, ErrorMessage = "Telefone inválido!")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Endereço é obrigatório!")]
        [MaxLength(200,  ErrorMessage = "Endereço deve ter no máximo 200 caracteres!")]
        public string Endereco { get; set; }
    }
}
using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GrillSystem.Dto
{
    public class ClienteDto
    {
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O CPF/CNPJ é obrigatório!")]
        [RegularExpression(@"(^\d{11}$)|(^\d{14}$)|(^\d{3}\.\d{3}\.\d{3}\-\d{2}$)|(^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$)", ErrorMessage = "O CPF/CNPJ está incoreto!")]
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

    public class ClienteUpdateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O telefone é obrigatório!")]
        [MinLength(10, ErrorMessage = "Telefone inválido!")]
        [MaxLength(15, ErrorMessage = "Telefone inválido!")]
        public string Telefone { get; set; }
        [Required(ErrorMessage = "Endereço é obrigatório!")]
        [MaxLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres!")]
        public string Endereco { get; set; }
    }
}
using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GrillSystem.Dto
{
    public class ClienteDto
    {
        [MaxLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "O CPF/CNPJ é obrigatório!")]
        [RegularExpression(@"(^\d{11}$)|(^\d{14}$)|(^\d{3}\.\d{3}\.\d{3}\-\d{2}$)|(^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$)", ErrorMessage = "O CPF/CNPJ está incoreto!")]
        public string CpfCnpj { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tipo do cliente é obrigatório!")]
        [EnumDataType(typeof(TipoCliente))]
        public TipoCliente? Tipo { get; set; }
        [Required(ErrorMessage = "O telefone é obrigatório!")]
        [MinLength(10, ErrorMessage = "Telefone inválido!")]
        [MaxLength(15, ErrorMessage = "Telefone inválido!")]
        public string Telefone { get; set; } = string.Empty;
        [MaxLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres!")]
        public string Endereco { get; set; } = string.Empty;
        [EmailAddress, MaxLength(250)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(15)]
        public string Celular { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Observacoes { get; set; } = string.Empty;
        [MaxLength(150)]
        public string? RazaoSocial { get; set; }
        [MaxLength(200)]
        public string? NomeFantasia { get; set; }
        public DateOnly? DataNascimento { get; set; }
    }

    public class ClienteUpdateDto
    {
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [MaxLength(150, ErrorMessage = "Nome deve ter no máximo 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "O telefone é obrigatório!")]
        [MinLength(10, ErrorMessage = "Telefone inválido!")]
        [MaxLength(15, ErrorMessage = "Telefone inválido!")]
        public string Telefone { get; set; } = string.Empty;
        [MaxLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres!")]
        public string Endereco { get; set; } = string.Empty;
        [EmailAddress, MaxLength(250)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(15)]
        public string Celular { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Observacoes { get; set; } = string.Empty;
        [EnumDataType(typeof(SituacaoCadastro))]
        public SituacaoCadastro Situacao { get; set; } = SituacaoCadastro.Ativo;
        [MaxLength(200)]
        public string? NomeFantasia { get; set; }
        public DateOnly? DataNascimento { get; set; }
    }
}

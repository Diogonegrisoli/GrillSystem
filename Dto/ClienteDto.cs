using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ClienteDto
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        [MinLength(11)]
        public string Cpf_Cnpj { get; set; }
        [Required]
        public TipoCliente Tipo { get; set; }
        [Required]
        [MinLength(11)]
        public string Telefone { get; set; }
        [Required]
        public string Endereco { get; set; }
    }
}

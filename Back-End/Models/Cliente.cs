using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrillSystem.Models
{
    [Table("cliente")]
    public class Cliente
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("nome")]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;
        [Column("cpf_cnpj")]
        [MaxLength(14)]
        public string CpfCnpj { get; set; } = string.Empty;
        [Column("tipo")]
        public TipoCliente Tipo { get; set; }
        [Column("telefone")]
        [MaxLength(15)]
        public string Telefone { get; set; } = string.Empty;
        [Column("endereco")]
        [MaxLength(200)]
        public string Endereco { get; set; } = string.Empty;

        public Cliente(string nome, string cpfCnpj, TipoCliente tipo, string telefone, string endereco)
        {
            Nome = nome;
            CpfCnpj = cpfCnpj;
            Tipo = tipo;
            Telefone = telefone;
            Endereco = endereco;
        }

        public Cliente() { }
    }

    public enum TipoCliente
    {
        Fisica,
        Juridica
    }
}

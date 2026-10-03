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
        [MaxLength(150)]
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
        public string Celular { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateOnly DataCadastro { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public SituacaoCadastro Situacao { get; set; } = SituacaoCadastro.Ativo;
        public string Observacoes { get; set; } = string.Empty;
        public PessoaFisica? PessoaFisica { get; set; }
        public PessoaJuridica? PessoaJuridica { get; set; }
        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();

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

    public enum SituacaoCadastro { Ativo, Inativo }
}

using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Nome { get; set; }
        [MaxLength(14)]
        public string CpfCnpj { get; set; }
        public TipoCliente Tipo { get; set; }
        [MaxLength(15)]
        public string  Telefone { get; set; }
        [MaxLength(200)]
        public string Endereco { get; set; }

        public Cliente(string nome,string cpfCnpj, TipoCliente tipo, string telefone, string endereco)
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
        Jurídica
    }
}

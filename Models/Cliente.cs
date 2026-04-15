namespace GrillSystem.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf_Cnpj { get; set; }
        public TipoCliente Tipo { get; set; }
        public string  Telefone { get; set; }
        public string Endereco { get; set; }

        public Cliente(string nome,string cpf_cnpj, TipoCliente tipo, string telefone, string endereco)
        {
            Nome = nome;
            Cpf_Cnpj = cpf_cnpj;
            Tipo = tipo;
            Telefone = telefone;
            Endereco = endereco;
        }
    }   
    
    public enum TipoCliente
    {
        Física,
        Jurídica
    }
}

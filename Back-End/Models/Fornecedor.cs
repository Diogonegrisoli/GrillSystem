namespace GrillSystem.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }
        public string NomeFantasia { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Contrato { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public SituacaoCadastro Situacao { get; set; } = SituacaoCadastro.Ativo;
        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();

        public Fornecedor() { }

        public Fornecedor(string razaoSocial, string nomeFantasia, string cnpj, string email, string endereco)
        {
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            Cnpj = cnpj;
            Email = email;
            Endereco = endereco;
        }
    }
}

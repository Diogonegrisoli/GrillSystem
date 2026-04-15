namespace GrillSystem.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }

        public Fornecedor(string razaoSocial, string cnpj, string email, string endereco)
        {
            RazaoSocial = razaoSocial;
            Cnpj = cnpj;
            Email = email;
            Endereco = endereco;
        }
    }    
}

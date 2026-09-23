namespace GrillSystem.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }

        public Produto() { }

        public Produto(string codigo, string descricao, decimal preco, int quantidade)
        {
            Codigo = codigo;
            Descricao = descricao;
            Preco = preco;
            Quantidade = quantidade;
        }
    }
}

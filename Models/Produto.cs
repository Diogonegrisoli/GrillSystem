namespace GrillSystem.Models
{
    public class Produto
    {
        public  int Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal Preco {  get; set; }
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

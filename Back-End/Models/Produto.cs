namespace GrillSystem.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public int EstoqueMinimo { get; set; }
        public UnidadeMedida UnidadeMedida { get; set; } = UnidadeMedida.Unidade;
        public SituacaoCadastro Situacao { get; set; } = SituacaoCadastro.Ativo;

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

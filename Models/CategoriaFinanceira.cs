namespace GrillSystem.Models
{
    public class CategoriaFinanceira
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public TipoCategoria Tipo { get; set; }

        public CategoriaFinanceira(){ }

        public CategoriaFinanceira(string nome, TipoCategoria tipo)
        {
            Nome = nome;
            Tipo = tipo;
        }
    }

    public enum TipoCategoria
    {
        Receita,
        Despesa
    }

}

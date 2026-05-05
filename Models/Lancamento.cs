namespace GrillSystem.Models
{
    public class Lancamento
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public DateOnly Data { get; set; }
        public string Descricao { get; set; }
        public int CategoriaFinanceiraId { get; set; }
        public CategoriaFinanceira CategoriaFinanceira { get; set; }
        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }

        public Lancamento() { }

        public Lancamento(decimal valor, DateOnly data, string descricao, int categoriaFinanceiraId, int funcionarioId)
        {
            Valor = valor;
            Data = data;
            Descricao = descricao;
            CategoriaFinanceiraId = categoriaFinanceiraId;
            FuncionarioId = funcionarioId;
        }
    }
}

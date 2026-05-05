namespace GrillSystem.Models
{
    public class MovimentacaoEstoque
    {
        public int Id { get; set; }
        public TipoMovimentacao Tipo { get; set; }
        public decimal Quantidade {  get; set; }
        public decimal CustoUnitario { get; set; }
        public DateOnly Data {  get; set; }
        public string Referencia { get; set; }
        public int MateriaPrimaId { get; set; }
        public MateriaPrima MateriaPrima { get; set; }

        public MovimentacaoEstoque()
        {
            
        }
        public MovimentacaoEstoque(TipoMovimentacao tipo, decimal quantidade, decimal custoUnitario,  DateOnly data, string referencia, int materiaPrimaId)
        {
            Tipo = tipo;
            Quantidade = quantidade;
            CustoUnitario = custoUnitario;
            Data = data;
            Referencia = referencia;
            MateriaPrimaId = materiaPrimaId;
        }
    }

    public enum TipoMovimentacao
    {
        Entrada = 1,
        Saida  = 2
    }
}

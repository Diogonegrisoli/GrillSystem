namespace GrillSystem.Models
{
    public class MateriaPrima
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal Quantidade { get; set; }
        public  decimal QuantidadeMinima { get; set; }
        public UnidadeMedida UnidadeMedida { get; set; }

        public MateriaPrima()
        {
            
        }

        public MateriaPrima(string codigo, string descricao, decimal quantidade, decimal quantidadeMinima, UnidadeMedida unidadeMedida)
        {
            Codigo = codigo;
            Descricao = descricao;
            Quantidade = quantidade;
            QuantidadeMinima = quantidadeMinima;
            UnidadeMedida = unidadeMedida;
        }

        public static string ObterSigla(UnidadeMedida unidade)
        {
            return unidade switch
            {
                UnidadeMedida.Grama => "g",
                UnidadeMedida.Quilograma => "kg",
                UnidadeMedida.Tonelada => "t",

                UnidadeMedida.Centimetro => "cm",
                UnidadeMedida.CentimetroQuadrado => "cm²",
                UnidadeMedida.CentimetroCubico => "cm³",
                UnidadeMedida.Metro => "m",
                UnidadeMedida.MetroQuadrado => "m²",
                UnidadeMedida.MetroCubico => "m³",
                UnidadeMedida.Mililitro => "mL",
                UnidadeMedida.Litro => "L",

                UnidadeMedida.Unidade => "un",
                UnidadeMedida.Caixa => "cx",
                UnidadeMedida.Pacote => "pct",

                _ => throw new ArgumentOutOfRangeException(nameof(unidade))
            };
        }
    }

    public enum UnidadeMedida
    {
        Grama,
        Quilograma,
        Tonelada,

        Centimetro,
        CentimetroQuadrado,
        CentimetroCubico,
        Metro,
        MetroQuadrado,
        MetroCubico,

        Mililitro,
        Litro,

        Unidade,
        Caixa,
        Pacote
    }
}

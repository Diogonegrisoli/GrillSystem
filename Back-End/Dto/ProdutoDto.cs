using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class ProdutoDto
    {
        [Required(ErrorMessage = "É necessário informar o código do produto!")]
        [MaxLength(50, ErrorMessage = "O código deve ter no máximo 50 caracteres!")]
        public string Codigo { get; set; } = string.Empty;
        [Required(ErrorMessage = "É necessário informar a descrição do produto!")]
        [MaxLength(250, ErrorMessage = "A descrição deve ter no máximo 250 caracteres!")]
        public string Descricao { get; set; } = string.Empty;
        [Required(ErrorMessage = "É necessário informar o preço do produto!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço não pode ser negativo!")]
        public decimal Preco { get; set; }
        [Range(0, int.MaxValue)]
        public int EstoqueMinimo { get; set; }
        [EnumDataType(typeof(GrillSystem.Models.UnidadeMedida))]
        public GrillSystem.Models.UnidadeMedida UnidadeMedida { get; set; } = GrillSystem.Models.UnidadeMedida.Unidade;
        [EnumDataType(typeof(GrillSystem.Models.SituacaoCadastro))]
        public GrillSystem.Models.SituacaoCadastro Situacao { get; set; } = GrillSystem.Models.SituacaoCadastro.Ativo;
    }
}

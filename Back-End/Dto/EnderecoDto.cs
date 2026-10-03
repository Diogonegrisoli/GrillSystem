using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto;

public sealed class EnderecoDto
{
    [Required, MaxLength(200)] public string Logradouro { get; set; } = string.Empty;
    [Required, MaxLength(20)] public string Numero { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string Bairro { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string Cidade { get; set; } = string.Empty;
    [Required, RegularExpression("^[A-Za-z]{2}$")] public string Estado { get; set; } = string.Empty;
    [Required, RegularExpression("^[0-9]{8}$")] public string Cep { get; set; } = string.Empty;
}

public enum TipoTitularEndereco { Cliente, Fornecedor, Funcionario }

public sealed class VinculoEnderecoDto
{
    [EnumDataType(typeof(TipoTitularEndereco))]
    public TipoTitularEndereco TipoTitular { get; set; }
    [Range(1, int.MaxValue)] public int TitularId { get; set; }
}

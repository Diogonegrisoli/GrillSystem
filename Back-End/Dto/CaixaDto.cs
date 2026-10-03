using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto;

public sealed class AbrirCaixaDto
{
    [Required, MaxLength(200)] public string Descricao { get; set; } = string.Empty;
    [Required, EnumDataType(typeof(TipoCaixa))] public TipoCaixa? Tipo { get; set; }
    [Range(0, double.MaxValue)] public decimal SaldoInicial { get; set; }
}

public sealed class LiquidarParcelaDto
{
    [Range(1, int.MaxValue)] public int ParcelaId { get; set; }
    [Required, EnumDataType(typeof(TipoMovimentacaoCaixa))]
    public TipoMovimentacaoCaixa? Tipo { get; set; }
    [MaxLength(300)] public string Descricao { get; set; } = string.Empty;
}

using GrillSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Dto
{
    public class LancamentoDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "O deve ser maior que zero!")]
        public decimal Valor { get; set; }
        [Required(ErrorMessage = "A data deve ser informada!")]
        public DateOnly Data { get; set; }
        [Required(ErrorMessage = "A descrição deve ser informada!")]
        [MinLength(30, ErrorMessage = "A descrição deve ter no mínimo 30 caracteres!")]
        [MaxLength(300, ErrorMessage = "A descrição deve ter no máximo 300 caracteres!")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "A categoria deve ser informada!")]
        public int CategoriaFinanceiraId { get; set; }
        [Required(ErrorMessage = "O funcionário deve ser informado!")]
        public int FuncionarioId { get; set; }
    }
}

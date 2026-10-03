using System.Text.Json.Serialization;

namespace GrillSystem.Models;

public class PessoaFisica
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    [JsonIgnore]
    public Cliente Cliente { get; set; } = null!;
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateOnly? DataNascimento { get; set; }
}

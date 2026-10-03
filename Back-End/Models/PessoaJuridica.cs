using System.Text.Json.Serialization;

namespace GrillSystem.Models;

public class PessoaJuridica
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    [JsonIgnore]
    public Cliente Cliente { get; set; } = null!;
    public string RazaoSocial { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
}

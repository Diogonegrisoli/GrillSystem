using System.Text.Json.Serialization;

namespace GrillSystem.Models;

public class Endereco
{
    public int Id { get; set; }
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    [JsonIgnore]
    public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    [JsonIgnore]
    public ICollection<Fornecedor> Fornecedores { get; set; } = new List<Fornecedor>();
    [JsonIgnore]
    public ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}

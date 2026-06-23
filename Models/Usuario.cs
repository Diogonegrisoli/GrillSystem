using System.ComponentModel.DataAnnotations;

namespace GrillSystem.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [MaxLength(150)]
        public string Email { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string SenhaHash { get; set; }
        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }

        public Usuario() { }

        public Usuario(string email, string senhaHash, int funcionarioId)
        {
            Email = email;
            SenhaHash = senhaHash;
            FuncionarioId = funcionarioId;
        }
    }
}

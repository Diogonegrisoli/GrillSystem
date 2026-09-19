using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace GrillSystem.Models
{
    public class Usuario : IdentityUser<int>
    {
        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; } = null!;

        [JsonIgnore]
        public override string? PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

        [JsonIgnore]
        public override string? SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }

        [JsonIgnore]
        public override string? ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }
    }
}

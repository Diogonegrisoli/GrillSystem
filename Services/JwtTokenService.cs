using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GrillSystem.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenDto Generate(Usuario usuario)
        {
            string key = _configuration["JWT_KEY"]
                ?? throw new InvalidOperationException("A variável JWT_KEY não foi configurada.");
            string issuer = _configuration["JWT_ISSUER"] ?? "GrillSystem";
            string audience = _configuration["JWT_AUDIENCE"] ?? "GrillSystem.Api";
            DateTime expiresAt = DateTime.UtcNow.AddHours(2);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim("funcionarioId", usuario.FuncionarioId.ToString())
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiraEm = expiresAt
            };
        }
    }
}

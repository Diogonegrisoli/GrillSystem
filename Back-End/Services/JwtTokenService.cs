using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GrillSystem.Services;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<Usuario> _userManager;

    public JwtTokenService(IConfiguration configuration, UserManager<Usuario> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<TokenDto> GenerateAsync(Usuario usuario)
    {
        string key = _configuration["JWT_KEY"]
            ?? _configuration["JWT_SECRET"]
            ?? throw new InvalidOperationException("A chave JWT não foi configurada.");
        string issuer = _configuration["JWT_ISSUER"] ?? "GrillSystem";
        string audience = _configuration["JWT_AUDIENCE"] ?? "GrillSystem.Api";
        int expirationMinutes = int.TryParse(_configuration["JWT_EXPIRATION_MINUTES"], out int minutes)
            && minutes is >= 1 and <= 1440
                ? minutes
                : 120;
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);
        var perfis = await _userManager.GetRolesAsync(usuario);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.UserName ?? usuario.Email ?? usuario.Id.ToString()),
            new("funcionarioId", usuario.FuncionarioId.ToString()),
            new("security_stamp", usuario.SecurityStamp ?? string.Empty)
        };
        claims.AddRange(perfis.Select(perfil => new Claim(ClaimTypes.Role, perfil)));

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: credentials);

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiraEm = expiresAt,
            Perfis = perfis.ToArray()
        };
    }
}

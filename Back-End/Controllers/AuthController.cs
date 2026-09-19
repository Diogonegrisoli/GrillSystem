using GrillSystem.Dto;
using GrillSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrillSystem.Controllers;

[Route("/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UsuarioServices _usuarioService;
    private readonly JwtTokenService _tokenService;

    public AuthController(UsuarioServices usuarioService, JwtTokenService tokenService)
    {
        _usuarioService = usuarioService;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto data)
    {
        var usuario = await _usuarioService.Authenticate(data);
        if (usuario is null)
        {
            return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
        }

        return Ok(await _tokenService.GenerateAsync(usuario));
    }

    [HttpGet("me")]
    public async Task<ActionResult<UsuarioResponseDto>> Me()
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int id))
        {
            return Unauthorized();
        }

        return Ok(await _usuarioService.GetId(id));
    }
}

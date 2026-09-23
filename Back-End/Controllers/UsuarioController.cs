using GrillSystem.Authorization;
using GrillSystem.Dto;
using GrillSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrillSystem.Controllers;

[Route("/usuario")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioServices _service;

    public UsuarioController(UsuarioServices service) => _service = service;

    [Authorize(Policy = Politicas.GerenciarUsuarios)]
    [HttpGet]
    public async Task<ActionResult<ResultadoPaginadoDto<UsuarioResponseDto>>> Get(
        [FromQuery] PaginacaoDto paginacao,
        CancellationToken cancellationToken) =>
        Ok(await _service.ListAll(paginacao, cancellationToken));

    [Authorize(Policy = Politicas.GerenciarUsuarios)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioResponseDto>> GetId(int id) =>
        Ok(await _service.GetId(id));

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<UsuarioResponseDto>> Create([FromBody] UsuarioDto data)
    {
        if (await _service.HasUsers() && !User.IsInRole(Perfis.Administrador))
        {
            return Forbid();
        }

        var usuario = await _service.Create(data);
        return CreatedAtAction(nameof(GetId), new { id = usuario.Id }, usuario);
    }

    [Authorize(Policy = Politicas.GerenciarUsuarios)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UsuarioResponseDto>> Update(int id, [FromBody] UsuarioUpdateDto data) =>
        Ok(await _service.Update(id, data));

    [Authorize(Policy = Politicas.GerenciarUsuarios)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        int? usuarioAtualId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int value)
            ? value
            : null;
        await _service.Delete(id, usuarioAtualId);
        return NoContent();
    }
}

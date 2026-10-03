using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers;

[ApiController]
[Route("endereco")]
public sealed class EnderecoController(EnderecoService service) : ControllerBase
{
    [HttpGet]
    public Task<ResultadoPaginadoDto<Endereco>> Listar([FromQuery] PaginacaoDto pagina, CancellationToken ct) =>
        service.Listar(pagina, ct);

    [HttpGet("{id:int}")]
    public Task<Endereco> Obter(int id, CancellationToken ct) => service.Obter(id, ct);

    [HttpPost]
    public async Task<ActionResult<Endereco>> Criar(EnderecoDto dto, CancellationToken ct) =>
        Ok(await service.Criar(dto, ct));

    [HttpPut("{id:int}")]
    public Task<Endereco> Alterar(int id, EnderecoDto dto, CancellationToken ct) => service.Alterar(id, dto, ct);

    [HttpPost("{id:int}/vinculos")]
    public async Task<IActionResult> Vincular(int id, VinculoEnderecoDto dto, CancellationToken ct)
    {
        await service.Vincular(id, dto, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}/vinculos")]
    public async Task<IActionResult> Desvincular(int id, VinculoEnderecoDto dto, CancellationToken ct)
    {
        await service.Desvincular(id, dto, ct);
        return NoContent();
    }

    [Authorize(Policy = Politicas.GerenciarSistema)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id, CancellationToken ct)
    {
        await service.Excluir(id, ct);
        return NoContent();
    }
}

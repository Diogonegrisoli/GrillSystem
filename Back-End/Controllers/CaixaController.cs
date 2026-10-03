using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers;

[ApiController]
[Authorize(Policy = Politicas.GerenciarFinanceiro)]
[Route("caixa")]
public sealed class CaixaController(CaixaService service) : ControllerBase
{
    [HttpGet]
    public Task<ResultadoPaginadoDto<Caixa>> Listar([FromQuery] PaginacaoDto pagina, CancellationToken ct) =>
        service.Listar(pagina, ct);

    [HttpGet("{id:int}")]
    public Task<Caixa> Obter(int id, CancellationToken ct) => service.Obter(id, ct);

    [HttpGet("{id:int}/movimentacoes")]
    public Task<ResultadoPaginadoDto<MovimentacaoCaixa>> Movimentacoes(
        int id, [FromQuery] PaginacaoDto pagina, CancellationToken ct) =>
        service.Movimentacoes(id, pagina, ct);

    [HttpPost]
    public Task<Caixa> Abrir(AbrirCaixaDto dto, CancellationToken ct) => service.Abrir(dto, ct);

    [HttpPost("{id:int}/liquidar-parcela")]
    public Task<MovimentacaoCaixa> Liquidar(int id, LiquidarParcelaDto dto, CancellationToken ct) =>
        service.Liquidar(id, dto, ct);

    [Authorize(Policy = Politicas.GerenciarSistema)]
    [HttpPost("{id:int}/fechar")]
    public Task<Caixa> Fechar(int id, CancellationToken ct) => service.Fechar(id, ct);
}

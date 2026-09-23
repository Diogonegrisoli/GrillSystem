using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("/movimentacao-estoque")]
    [ApiController]
    public class MovimentacaoEstoqueController : ControllerBase
    {
        private readonly MovimentacaoEstoqueService _service;
        public MovimentacaoEstoqueController(MovimentacaoEstoqueService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResultadoPaginadoDto<MovimentacaoEstoque>> Get(
            [FromQuery] PaginacaoDto paginacao,
            CancellationToken cancellationToken)
        {
            try
            {
                return await _service.ListAll(paginacao, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<MovimentacaoEstoque> GetId(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _service.GetId(id, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<MovimentacaoEstoque>> Create(
            [FromBody] MovimentacaoEstoqueDto data,
            CancellationToken cancellationToken)
        {
            try
            {
                return await _service.Create(data, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MovimentacaoEstoque>> Update(
            int id,
            MovimentacaoEstoqueDto data,
            CancellationToken cancellationToken)
        {
            try
            {
                return await _service.Update(id, data, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Policy = Politicas.GerenciarSistema)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MovimentacaoEstoque>> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _service.Delete(id, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

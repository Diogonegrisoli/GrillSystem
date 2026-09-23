using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("/lancamento")]
    [ApiController]
    public class LancamentoController : ControllerBase
    {
        private readonly LancamentoService _service;
        public LancamentoController(LancamentoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResultadoPaginadoDto<Lancamento>> Get(
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
        public async Task<Lancamento> GetId(int id)
        {
            try
            {
                return await _service.GetId(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Lancamento>> Create([FromBody] LancamentoDto data)
        {
            try
            {
                return await _service.Create(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Lancamento>> Update(int id, LancamentoDto data)
        {
            try
            {
                return await _service.Update(id, data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Policy = Politicas.GerenciarSistema)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Lancamento>> Delete(int id)
        {
            try
            {
                return await _service.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

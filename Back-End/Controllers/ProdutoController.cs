using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("/produto")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoServices _service;
        public ProdutoController(ProdutoServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResultadoPaginadoDto<Produto>> Get(
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
        public async Task<Produto> GetId(int id)
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
        public async Task<ActionResult<Produto>> Create([FromBody] ProdutoDto data)
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
        public async Task<ActionResult<Produto>> Update(int id, ProdutoDto data)
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
        public async Task<ActionResult<Produto>> Delete(int id)
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

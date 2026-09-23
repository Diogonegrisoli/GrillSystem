using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("/produto-materia-prima")]
    [ApiController]
    public class ProdutoMateriaPrimaController : ControllerBase
    {
        private readonly ProdutoMateriaPrimaServices _service;
        public ProdutoMateriaPrimaController(ProdutoMateriaPrimaServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResultadoPaginadoDto<ProdutoMateriaPrima>> Get(
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
        public async Task<ProdutoMateriaPrima> GetId(int id)
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
        public async Task<ActionResult<ProdutoMateriaPrima>> Create(
            [FromBody] ProdutoMateriaPrimaDto data,
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
        public async Task<ActionResult<ProdutoMateriaPrima>> Update(
            int id,
            ProdutoMateriaPrimaDto data,
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
        public async Task<ActionResult<ProdutoMateriaPrima>> Delete(int id, CancellationToken cancellationToken)
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

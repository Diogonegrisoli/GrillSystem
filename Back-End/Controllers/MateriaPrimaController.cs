using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("/materia-prima")]
    [ApiController]
    public class MateriaPrimaController : ControllerBase
    {
        private readonly MateriaPrimaServices _service;
        public MateriaPrimaController(MateriaPrimaServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResultadoPaginadoDto<MateriaPrima>> Get(
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
        public async Task<MateriaPrima> GetId(int id)
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
        public async Task<ActionResult<MateriaPrima>> Create([FromBody] MateriaPrimaDto data)
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
        public async Task<ActionResult<MateriaPrima>> Update(int id, MateriaPrimaDto data)
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
        public async Task<ActionResult<MateriaPrima>> Delete(int id)
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

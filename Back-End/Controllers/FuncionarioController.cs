using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("/funcionario")]
    [ApiController]
    public class FuncionarioController : ControllerBase
    {
        private readonly FuncionarioServices _service;
        public FuncionarioController(FuncionarioServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResultadoPaginadoDto<Funcionario>> Get(
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
        public async Task<Funcionario> GetId(int id)
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
        public async Task<ActionResult<Funcionario>> Create([FromBody] FuncionarioDto data)
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
        public async Task<ActionResult<Funcionario>> Update(int id, FuncionarioDto data)
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
        public async Task<ActionResult<Funcionario>> Delete(int id)
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

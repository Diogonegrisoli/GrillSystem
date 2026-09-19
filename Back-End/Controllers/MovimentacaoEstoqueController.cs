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
        public async Task<ICollection<MovimentacaoEstoque>> Get()
        {
            try
            {
                return await _service.ListAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<MovimentacaoEstoque> GetId(int id)
        {
            try
            {
                return await _service.GetId(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<MovimentacaoEstoque>> Create([FromBody] MovimentacaoEstoqueDto data)
        {
            try
            {
                return await _service.Create(data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MovimentacaoEstoque>> Update(int id, MovimentacaoEstoqueDto data)
        {
            try
            {
                return await _service.Update(id, data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Policy = Politicas.GerenciarSistema)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MovimentacaoEstoque>> Delete(int id)
        {
            try
            {
                return await _service.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

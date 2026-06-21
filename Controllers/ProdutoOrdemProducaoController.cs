using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("/produto-ordem-producao")]
    [ApiController]
    public class ProdutoOrdemProducaoController : ControllerBase
    {
        private readonly ProdutoOrdemProducaoServices _service;
        public ProdutoOrdemProducaoController(ProdutoOrdemProducaoServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ICollection<ProdutoOrdemProducao>> Get()
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
        public async Task<ProdutoOrdemProducao> GetId(int id)
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
        public async Task<ActionResult<ProdutoOrdemProducao>> Create([FromBody] ProdutoOrdemProducaoDto data)
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
        public async Task<ActionResult<ProdutoOrdemProducao>> Update(int id, ProdutoOrdemProducaoDto data)
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoOrdemProducao>> Delete(int id)
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

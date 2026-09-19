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
        public async Task<ICollection<ProdutoMateriaPrima>> Get()
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
        public async Task<ProdutoMateriaPrima> GetId(int id)
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
        public async Task<ActionResult<ProdutoMateriaPrima>> Create([FromBody] ProdutoMateriaPrimaDto data)
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
        public async Task<ActionResult<ProdutoMateriaPrima>> Update(int id, ProdutoMateriaPrimaDto data)
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
        public async Task<ActionResult<ProdutoMateriaPrima>> Delete(int id)
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

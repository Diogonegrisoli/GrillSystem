using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("/fornecedor-materia-prima")]
    [ApiController]
    public class FornecedorMateriaPrimaController : ControllerBase
    {
        private readonly FornecedorMateriaPrimaServices _service;

        public FornecedorMateriaPrimaController(FornecedorMateriaPrimaServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ICollection<FornecedorMateriaPrima>> Get()
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
        public async Task<FornecedorMateriaPrima> GetId(int id)
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
        public async Task<ActionResult<FornecedorMateriaPrima>> Create([FromBody] FornecedorMateriaPrimaDto data)
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
        public async Task<ActionResult<FornecedorMateriaPrima>> Update(int id, FornecedorMateriaPrimaDto data)
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
        public async Task<ActionResult<FornecedorMateriaPrima>> Delete(int id)
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

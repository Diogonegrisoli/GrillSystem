using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrillSystem.Controllers
{
    [Route("/conta-receber")]
    [ApiController]
    public class ContaReceberController : ControllerBase
    {
        private readonly ContaReceberServices _service;
        public ContaReceberController(ContaReceberServices service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ICollection<ContaReceber>> Get()
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
        public async Task<ContaReceber> GetId(int id)
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
        public async Task<ActionResult<ContaReceber>> Create([FromBody] ContaReceberDto data)
        {         
                return await _service.Create(data);   
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ContaReceber>> Update(int id, ContaReceberUpdateDto data)
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
        public async Task<ActionResult<ContaReceber>> Delete(int id)
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

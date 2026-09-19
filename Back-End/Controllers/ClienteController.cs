using GrillSystem.Data;
using GrillSystem.Models;
using GrillSystem.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrillSystem.Services;

namespace GrillSystem.Controllers
{
    [Route("/cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteServices _service;

        public ClienteController(ClienteServices service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ICollection<Cliente>> ListAll()
        {
            var cliente = await _service.ListAll();
            return cliente;
        }


        [HttpGet("{id}")]
        public async Task<Cliente> GetId(int id)
        {

            var cliente = await _service.GetId(id);

            return cliente;

        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Create([FromBody] ClienteDto data)
        {
            await _service.Create(data);

            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto data)
        {
            try
            {
                await _service.Update(id, data);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Policy = Politicas.GerenciarSistema)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

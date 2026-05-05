using GrillSystem.Data;
using GrillSystem.Models;
using GrillSystem.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrillSystem.Services;

namespace GrillSystem.Controllers
{
    [Route("/Cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ClienteServices _service;

        public ClienteController(AppDbContext context, ClienteServices service)
        {
            _context = context;
            _service = service;
        }


        [HttpGet]
        public async Task<ICollection<Cliente>> ListAll()
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
        public async Task<Cliente> GetId(int id)
        {
            try
            {
                var cliente = await _service.GetId(id);

                return cliente;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Create([FromBody] ClienteDto data)
        {
            try
            {
                await _service.Create(data);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteDto data)
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

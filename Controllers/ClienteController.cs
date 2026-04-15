using GrillSystem.Data;
using GrillSystem.Models;
using GrillSystem.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Controllers
{
    [Route("/Cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ICollection<Cliente>> ListAll()
        {
            try
            {
                return await _context.Clientes.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("{id}")]
        public async Task<Cliente> GetId(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

                if (cliente is null)
                {
                    throw new Exception($"O cliente com o {id}# não foi localizado.");
                }

                return cliente;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Create([FromBody] ClienteDto data)
        {
            var cliente = new Cliente
            (data.Nome, data.Cpf_Cnpj, data.Tipo, data.Endereco, data.Telefone);

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return Ok(cliente);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteDto data)
        {
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
                if (cliente is null)
                {
                    throw new Exception($"O cliente com o {id}# não foi localizado.");
                }

                cliente.Cpf_Cnpj = data.Cpf_Cnpj;
                cliente.Nome = data.Nome;
                cliente.Endereco = data.Endereco;
                cliente.Telefone = data.Telefone;
                cliente.Tipo = data.Tipo;

                await _context.SaveChangesAsync();

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            if (cliente is null)
            {
                throw new Exception($"O cliente com o {id}# não foi localizado.");
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

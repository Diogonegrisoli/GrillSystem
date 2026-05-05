using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class ClienteServices
    {
        private readonly AppDbContext _context;

        public ClienteServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Cliente>> ListAll()
        {
            try
            {
                return await _context.Clientes.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível listar os clientes.");
            }
        }

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
        public async Task<ActionResult<Cliente>> Create([FromBody] ClienteDto data)
        {
            var cliente = new Cliente
            (data.Nome, data.CpfCnpj, data.Tipo, data.Endereco, data.Telefone);

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return cliente;
        }

        public async Task<Cliente> Update(int id, [FromBody] ClienteDto data)
        {
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
                if (cliente is null)
                {
                    throw new Exception($"O cliente com o {id}# não foi localizado.");
                }

                cliente.CpfCnpj = data.CpfCnpj;
                cliente.Nome = data.Nome;
                cliente.Endereco = data.Endereco;
                cliente.Telefone = data.Telefone;
                cliente.Tipo = data.Tipo;

                await _context.SaveChangesAsync();

                return cliente;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Cliente> Delete(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            if (cliente is null)
            {
                throw new Exception($"O cliente com o {id}# não foi localizado.");
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return cliente;
        }
    }
}

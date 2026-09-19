using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Validacao;
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
                var clientes = await _context.Clientes.ToListAsync();

                if (clientes.Count() < 1)
                {
                    throw new Exception("Não foi possível retornar um usuário!");
                }

                return clientes;
            }
            catch (Exception ex)
            {
                throw;
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
        public async Task<Cliente> Create(ClienteDto data)
        {
            try
            {
                string cpfCnpj = Validacoes.ValidarCpfCnpj(data.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""));
                string telefone = new string(data.Telefone.Where(char.IsDigit).ToArray());
                string nome = data.Nome.Trim();

                if(data.Tipo == TipoCliente.Fisica && cpfCnpj.Length != 11)
                {
                    throw new Exception("O CPF digitado é inválido!");
                }

                if(data.Tipo == TipoCliente.Juridica && cpfCnpj.Length != 14)
                {
                    throw new Exception("O CNPJ digitado é inválido!");
                }

                int cpfCnpjExiste = _context.Clientes.Count(x => x.CpfCnpj == cpfCnpj);

                if(cpfCnpjExiste > 0)
                {
                    throw new Exception("O CPF/CNPJ informado já existe!");
                }

                var cliente = new Cliente
                            (nome, cpfCnpj, data.Tipo, telefone, data.Endereco);

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                return cliente;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Cliente> Update(int id, ClienteUpdateDto data)
        {
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
                if (cliente is null)
                {
                    throw new Exception($"O cliente com o {id}# não foi localizado.");
                }
                cliente.Nome = data.Nome;
                cliente.Endereco = data.Endereco;
                cliente.Telefone = data.Telefone;

                await _context.SaveChangesAsync();

                return cliente;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível atualizar o cliente.", ex);
            }
        }

        public async Task<Cliente> Delete(int id)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Não foi possível deletar o cliente.", ex);
            }

        }
    }
}

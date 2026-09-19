using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Validacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class FornecedorService
    {
        private readonly AppDbContext _context;
        public FornecedorService(AppDbContext context)
        {
            _context = context;   
        }

        public async Task<ICollection<Fornecedor>> ListAll()
        {
            try
            {
                var fornecedor = await _context.Fornecedores.ToListAsync();
                if (fornecedor.Count() < 1)
                {
                    throw new Exception("Não foi possível retornar nenhum fornecedor!");
                }

                return fornecedor;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Fornecedor> GetId(int id)
        {
            try
            {
                var fornecedor = await _context.Fornecedores.FirstOrDefaultAsync(x => x.Id == id);
                if (fornecedor is null)
                {
                    throw new Exception($"O fornecedor com o id {id}# não foi localizado!");
                }
                return fornecedor;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Fornecedor> Create([FromBody] FornecedorDto data)
        {
            try
            {
                string cnpjInserido = data.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                string cnpj = Validacoes.ValidarCnpj(cnpjInserido);

                var fornecedor = new Fornecedor
                (data.RazaoSocial, data.NomeFantasia, cnpj, data.Endereco, data.Email);

                _context.Fornecedores.Add(fornecedor);
                await _context.SaveChangesAsync();

                return fornecedor;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Fornecedor> Update(int id, [FromBody] FornecedorUpdateDto data)
        {
            try
            {
                var fornecedor = await _context.Fornecedores.FirstOrDefaultAsync(x => x.Id == id);
                if (fornecedor is null)
                {
                    throw new Exception($"A movimentação no estoque com o id {id}# não foi localizado!");
                }

                fornecedor.RazaoSocial = data.RazaoSocial;
                fornecedor.NomeFantasia = data.NomeFantasia;
                fornecedor.Email = data.Email;
                fornecedor.Endereco = data.Endereco;

                await _context.SaveChangesAsync();

                return fornecedor;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível atualizar o fornecedor.", ex);
            }
        }

        public async Task<Fornecedor> Delete(int id)
        {
            try
            {
                var fornecedor = await _context.Fornecedores.FirstOrDefaultAsync(x => x.Id == id);
                if (fornecedor is null)
                {
                    throw new Exception($"O fornecedor com o id {id}# não foi localizado!");
                }

                _context.Fornecedores.Remove(fornecedor);
                await _context.SaveChangesAsync();

                return fornecedor;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível deletar o fornecedor.", ex);
            }
        }
    }
}

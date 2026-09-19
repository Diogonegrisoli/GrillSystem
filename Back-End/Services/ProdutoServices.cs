using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class ProdutoServices
    {
        private readonly AppDbContext _context;
        public ProdutoServices(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ICollection<Produto>> ListAll()
        {
            try
            {
                var produto = await _context.Produtos.ToListAsync();
                if (produto is null)
                {
                    throw new Exception("Não foi possível retornar nenhum produto!");
                }

                return produto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Produto> GetId(int id)
        {
            try
            {
                var produto = await _context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
                if (produto is null)
                {
                    throw new Exception($"O produto com o id {id}# não foi localizado!");
                }
                return produto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Produto> Create([FromBody] ProdutoDto data)
        {
            try
            {
                var produto = new Produto
                (data.Codigo, data.Descricao, data.Preco, data.Quantidade);

                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                return produto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Produto> Update(int id, [FromBody] ProdutoDto data)
        {
            try
            {
                var produto = await _context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
                if (produto is null)
                {
                    throw new Exception($"O produto com o id {id}# não foi localizado!");
                }

                produto.Codigo = data.Codigo;
                produto.Descricao = data.Descricao;
                produto.Preco = data.Preco;
                produto.Quantidade = data.Quantidade;

                await _context.SaveChangesAsync();


                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível atualizar o produto.", ex);
            }
        }

        public async Task<Produto> Delete(int id)
        {
            try
            {
                var produto = await _context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
                if (produto is null)
                {
                    throw new Exception($"O produto com o id {id}# não foi localizado!");
                }

                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();

                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível deletar o produto.", ex);
            }
        }
    }
}

using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class ProdutoOrdemProducaoServices
    {
        private readonly AppDbContext _context;
        public ProdutoOrdemProducaoServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ProdutoOrdemProducao>> ListAll()
        {
            try
            {
                var produto = await _context.ProdutosOrdensProducao.ToListAsync();
                if (produto is null)
                {
                    throw new Exception("Não foi possível retornar nenhum produto!");
                }

                return produto;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<ProdutoOrdemProducao> GetId(int id)
        {
            try
            {
                var produto = await _context.ProdutosOrdensProducao.FirstOrDefaultAsync(x => x.Id == id);
                if (produto is null)
                {
                    throw new Exception($"O produto com o id {id}# não foi localizado!");
                }
                return produto;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<ProdutoOrdemProducao> Create([FromBody] ProdutoOrdemProducaoDto data)
        {
            var produto = new ProdutoOrdemProducao
            (data.ProdutoId, data.OrdemProducaoId);

            _context.ProdutosOrdensProducao.Add(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<ProdutoOrdemProducao> Update(int id, [FromBody] ProdutoOrdemProducaoDto data)
        {
            var produto = await _context.ProdutosOrdensProducao.FirstOrDefaultAsync(x => x.Id == id);
            if (produto is null)
            {
                throw new Exception($"O produto com o id {id}# não foi localizado!");
            }

            produto.ProdutoId = data.ProdutoId;
            produto.OrdemProducaoId = data.OrdemProducaoId;

            await _context.SaveChangesAsync();


            return produto;
        }

        public async Task<ProdutoOrdemProducao> Delete(int id)
        {
            var produto = await _context.ProdutosOrdensProducao.FirstOrDefaultAsync(x => x.Id == id);
            if (produto is null)
            {
                throw new Exception($"O produto com o id {id}# não foi localizado!");
            }

            _context.ProdutosOrdensProducao.Remove(produto);
            await _context.SaveChangesAsync();

            return produto;
        }
    }
}

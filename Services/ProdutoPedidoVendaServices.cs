using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class ProdutoPedidoVendaServices
    {
        private readonly AppDbContext _context;
        public ProdutoPedidoVendaServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ProdutoPedidoVenda>> ListAll()
        {
            try
            {
                var produto = await _context.ProdutosPedidosVenda.ToListAsync();
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

        public async Task<ProdutoPedidoVenda> GetId(int id)
        {
            try
            {
                var produto = await _context.ProdutosPedidosVenda.FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<ProdutoPedidoVenda> Create([FromBody] ProdutoPedidoVendaDto data)
        {
            var produto = new ProdutoPedidoVenda
            (data.ProdutoId, data.PedidoVendaId);

            _context.ProdutosPedidosVenda.Add(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<ProdutoPedidoVenda> Update(int id, [FromBody] ProdutoPedidoVendaDto data)
        {
            var produto = await _context.ProdutosPedidosVenda.FirstOrDefaultAsync(x => x.Id == id);
            if (produto is null)
            {
                throw new Exception($"O produto com o id {id}# não foi localizado!");
            }

            produto.ProdutoId = data.ProdutoId;
            produto.PedidoVendaId = data.PedidoVendaId;

            await _context.SaveChangesAsync();


            return produto;
        }

        public async Task<ProdutoPedidoVenda> Delete(int id)
        {
            var produto = await _context.ProdutosPedidosVenda.FirstOrDefaultAsync(x => x.Id == id);
            if (produto is null)
            {
                throw new Exception($"O produto com o id {id}# não foi localizado!");
            }

            _context.ProdutosPedidosVenda.Remove(produto);
            await _context.SaveChangesAsync();

            return produto;
        }
    }
}

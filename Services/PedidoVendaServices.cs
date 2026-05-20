using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class PedidoVendaServices
    {
        private readonly AppDbContext _context;
        public PedidoVendaServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<PedidoVenda>> ListAll()
        {
            try
            {
                var pedido = await _context.PedidosVenda.ToListAsync();
                if (pedido is null)
                {
                    throw new Exception("Não foi possível retornar nenhum pedido de venda!");
                }

                return pedido;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<PedidoVenda> GetId(int id)
        {
            try
            {
                var pedido = await _context.PedidosVenda.FirstOrDefaultAsync(x => x.Id == id);
                if (pedido is null)
                {
                    throw new Exception($"O pedido de venda com o id {id}# não foi localizado!");
                }
                return pedido;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<PedidoVenda> Create([FromBody] PedidoVendaDto data)
        {
            var pedido = new PedidoVenda
            (data.DataPedido, data.DataEntrega, data.ValorTotal, data.ClienteId);

            _context.PedidosVenda.Add(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }

        public async Task<PedidoVenda> Update(int id, [FromBody] PedidoVendaUpdateDto data)
        {
            var pedido = await _context.PedidosVenda.FirstOrDefaultAsync(x => x.Id == id);
            if (pedido is null)
            {
                throw new Exception($"O pedido de venda com o id {id}# não foi localizado!");
            }

            pedido.DataPedido = data.DataPedido;
            pedido.DataEntrega = data.DataEntrega;
            pedido.Status = data.Status;
            pedido.ValorTotal = data.ValorTotal;
            pedido.ClienteId = data.ClienteId;

            await _context.SaveChangesAsync();


            return pedido;
        }

        public async Task<PedidoVenda> Delete(int id)
        {
            var pedido = await _context.PedidosVenda.FirstOrDefaultAsync(x => x.Id == id);
            if (pedido is null)
            {
                throw new Exception($"O pedido de venda com o id {id}# não foi localizado!");
            }

            _context.PedidosVenda.Remove(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }
    }
}

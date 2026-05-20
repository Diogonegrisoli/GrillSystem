using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class PedidoCompraService
    {
        private readonly AppDbContext _context;
        public PedidoCompraService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<PedidoCompra>> ListAll()
        {
            var pedidoCompra = await _context.PedidosCompra.ToListAsync();
            if (pedidoCompra is null)
            {
                throw new Exception("Não foi possível retornar nenhum pedido de compra!");
            }

            return pedidoCompra;
        }

        public async Task<PedidoCompra> GetId(int id)
        {
            var pedidoCompra = await _context.PedidosCompra.FirstOrDefaultAsync(x => x.Id == id);
            if (pedidoCompra is null)
            {
                throw new Exception("Não foi possível retornar nenhum pedido de compra!");
            }

            return pedidoCompra;
        }

        public async Task<PedidoCompra> Create([FromBody] PedidoCompraDto data)
        {
            var pedidoCompra = new PedidoCompra
                (data.DataPedido, data.DataEntrega, data.ValorTotal, data.FornecedorId, data.FuncionarioId);

            _context.PedidosCompra.Add(pedidoCompra);
            await _context.SaveChangesAsync();

            return pedidoCompra;
        }

        public async Task<PedidoCompra> Update(int id, [FromBody] PedidoCompraUpdateDto data)
        {
            var pedidoCompra = await _context.PedidosCompra.FirstOrDefaultAsync(x => x.Id == id);
            if (pedidoCompra is null)
            {
                throw new Exception("Não foi possível retornar nenhum pedido de compra!");
            }

            pedidoCompra.DataPedido = data.DataPedido;
            pedidoCompra.DataEntrega = data.DataEntrega;
            pedidoCompra.Status = data.Status;
            pedidoCompra.ValorTotal = data.ValorTotal;
            pedidoCompra.FornecedorId = data.FornecedorId;
            pedidoCompra.FuncionarioId = data.FuncionarioId;

            await _context.SaveChangesAsync();

            return pedidoCompra;
        }

        public async Task Delete(int id)
        {
            var pedidoCompra = await _context.PedidosCompra.FirstOrDefaultAsync(x => x.Id == id);
            if (pedidoCompra is null)
            {
                throw new Exception("Não foi possível retornar nenhum pedido de compra!");
            }

            _context.PedidosCompra.Remove(pedidoCompra);
            await _context.SaveChangesAsync();
        }
    }
}

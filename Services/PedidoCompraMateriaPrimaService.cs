using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class PedidoCompraMateriaPrimaService
    {
        private readonly AppDbContext _context;
        public PedidoCompraMateriaPrimaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<PedidoCompraMateriaPrima>> ListAll()
        {
            var pedidoMateria = await _context.PedidosCompraMateriasPrimas.ToListAsync();
            if (pedidoMateria is null)
            {
                throw new Exception("Não foi possível retornar nenhum pedido de compra/matéria-prima!");
            }

            return pedidoMateria;
        }

        public async Task<PedidoCompraMateriaPrima> GetId(int id)
        {
            var pedidoMateria = await _context.PedidosCompraMateriasPrimas.FirstOrDefaultAsync(x => x.Id == id);
            if (pedidoMateria is null)
            {
                throw new Exception("Não foi possível retornar nenhum pedido de compra/matéria-prima!");
            }

            return pedidoMateria;
        }

        public async Task<PedidoCompraMateriaPrima> Create([FromBody] PedidoCompraMateriaPrimaDto data)
        {
            var pedidoMateria = new PedidoCompraMateriaPrima
                (data.PedidoCompraId, data.MateriaPrimaId);

            _context.PedidosCompraMateriasPrimas.Add(pedidoMateria);
            await _context.SaveChangesAsync();

            return pedidoMateria;
        }

        public async Task<PedidoCompraMateriaPrima> Update(int id, [FromBody] PedidoCompraMateriaPrimaDto data)
        {
            var pedidoMateria = await _context.PedidosCompraMateriasPrimas.FirstOrDefaultAsync(x => x.Id == id);
            if (pedidoMateria is null)
            {
                throw new Exception("Não foi possível retornar nenhum pedido de compra/matéria-prima!");
            }

            pedidoMateria.PedidoCompraId = data.PedidoCompraId;
            pedidoMateria.MateriaPrimaId = data.MateriaPrimaId;

            await _context.SaveChangesAsync();

            return pedidoMateria;
        }

        public async Task Delete(int id)
        {
            var pedidoMateria = await _context.PedidosCompraMateriasPrimas.FirstOrDefaultAsync(x => x.Id == id);
            if (pedidoMateria is null)
            {
                throw new Exception("Não foi possível retornar nenhum pedido de compra/matéria-prima!");
            }

            _context.PedidosCompraMateriasPrimas.Remove(pedidoMateria);
            await _context.SaveChangesAsync();
        }
    }
}

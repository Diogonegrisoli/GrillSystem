using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class ContaPagarService
    {
        private readonly AppDbContext _context;
        public ContaPagarService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ContaPagar>> ListAll()
        {
            var contaPagar = await _context.ContasPagar.ToListAsync();
            if (contaPagar is null)
            {
                throw new Exception("Não foi possível retornar nenhuma conta a pagar!");
            }

            return contaPagar;
        }

        public async Task<ContaPagar> GetId(int id)
        {
            var contaPagar = await _context.ContasPagar.FirstOrDefaultAsync(x => x.Id == id);
            if (contaPagar is null)
            {
                throw new Exception("Não foi possível retornar nenhuma conta a pagar!");
            }

            return contaPagar;
        }

        public async Task<ContaPagar> Create([FromBody] ContaPagarDto data)
        {
            var contaPagarr = new ContaPagar
                (data.Valor, data.TipoPagamento, data.DataEmissao, data.DataVencimento, data.DataPagamento, data.PedidoCompraId);

            _context.ContasPagar.Add(contaPagarr);
            await _context.SaveChangesAsync();

            return contaPagarr;
        }

        public async Task<ContaPagar> Update(int id, [FromBody] ContaPagarDto data)
        {
            var contaPagar = await _context.ContasPagar.FirstOrDefaultAsync(x => x.Id == id);
            if (contaPagar is null)
            {
                throw new Exception("Não foi possível retornar nenhuma conta a pagar!");
            }

            contaPagar.Valor = data.Valor;
            contaPagar.TipoPagamento = data.TipoPagamento;
            contaPagar.DataEmissao = data.DataEmissao;
            contaPagar.DataVencimento = data.DataVencimento;
            contaPagar.DataPagamento = data.DataPagamento;
            contaPagar.PedidoCompraId = data.PedidoCompraId;

            await _context.SaveChangesAsync();

            return contaPagar;
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

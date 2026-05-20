using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class ContaReceberServices
    {
        private readonly AppDbContext _context;
        public ContaReceberServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ContaReceber>> ListAll()
        {
            try
            {
                var conta = await _context.ContasReceber.ToListAsync();
                if (conta is null)
                {
                    throw new Exception("Não foi possível retornar nenhum funcionário!");
                }

                return conta;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<ContaReceber> GetId(int id)
        {
            try
            {
                var conta = await _context.ContasReceber.FirstOrDefaultAsync(x => x.Id == id);
                if (conta is null)
                {
                    throw new Exception($"O funcionário com o id {id}# não foi localizado!");
                }
                return conta;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<ContaReceber> Create([FromBody] ContaReceberDto data)
        {
            var conta = new ContaReceber
            (data.Valor, data.DataVencimento, data.TipoPagamento, data.PedidoVendaId);

            _context.ContasReceber.Add(conta);
            await _context.SaveChangesAsync();

            return conta;
        }

        public async Task<ContaReceber> Update(int id, [FromBody] ContaReceberUpdateDto data)
        {
            var conta = await _context.ContasReceber.FirstOrDefaultAsync(x => x.Id == id);
            if (conta is null)
            {
                throw new Exception($"O funcionário com o id {id}# não foi localizado!");
            }

            conta.Valor = data.Valor;
            conta.DataVencimento = data.DataVencimento;
            conta.DataRecebimento = data.DataRecebimento;
            conta.StatusPagamento = data.StatusPagamento;
            conta.TipoPagamento = data.TipoPagamento;
            conta.PedidoVendaId = data.PedidoVendaId;

            await _context.SaveChangesAsync();


            return conta;
        }

        public async Task<ContaReceber> Delete(int id)
        {
            var conta = await _context.ContasReceber.FirstOrDefaultAsync(x => x.Id == id);
            if (conta is null)
            {
                throw new Exception($"O funcionário com o id {id}# não foi localizado!");
            }

            _context.ContasReceber.Remove(conta);
            await _context.SaveChangesAsync();

            return conta;
        }
    }
}

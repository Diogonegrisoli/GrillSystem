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
                throw;
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
                throw;
            }
        }

        public async Task<ContaReceber> Create([FromBody] ContaReceberDto data)
        {
            try
            {
               
                if (data.DataVencimento < DateOnly.FromDateTime(DateTime.Today))
                {
                    throw new Exception("A data do vencimento não pode ser menor que a data atual!");
                }

                if(data.DataRecebimento.HasValue && data.DataRecebimento < DateOnly.FromDateTime(DateTime.Today))
                {
                    throw new Exception("A data do recebimento não pode ser menor que a data da emissão!");
                }

                StatusPagamento statusPagamento = StatusPagamento.Pendente;
                if (data.DataRecebimento.HasValue)
                {
                    statusPagamento = StatusPagamento.Pago;
                }

                var conta = new ContaReceber
                (data.Valor, data.DataVencimento, statusPagamento, data.DataRecebimento, data.TipoPagamento, data.PedidoVendaId);

                _context.ContasReceber.Add(conta);
                await _context.SaveChangesAsync();

                return conta;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ContaReceber> Update(int id, [FromBody] ContaReceberUpdateDto data)
        {
            try
            {
                var conta = await _context.ContasReceber.FirstOrDefaultAsync(x => x.Id == id);
                if (conta is null)
                {
                    throw new Exception($"A conta a receber com o id {id} não foi localizada!");
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
            catch (Exception ex)
            {
                throw new Exception("Não foi possível atualizar a conta a receber.", ex);
            }
        }

        public async Task<ContaReceber> Delete(int id)
        {
            try
            {
                var conta = await _context.ContasReceber.FirstOrDefaultAsync(x => x.Id == id);
                if (conta is null)
                {
                    throw new Exception($"A conta a receber com o id {id} não foi localizada!");
                }

                _context.ContasReceber.Remove(conta);
                await _context.SaveChangesAsync();

                return conta;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível deletar a conta a receber.", ex);
            }
        }
    }
}

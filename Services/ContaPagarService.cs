using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using GrillSystem.Validacao;
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
            try
            {
                var contaPagar = await _context.ContasPagar.ToListAsync();
                if (contaPagar is null)
                {
                    throw new Exception("Não foi possível retornar nenhuma conta a pagar!");
                }

                return contaPagar;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ContaPagar> GetId(int id)
        {
            try
            {
                var contaPagar = await _context.ContasPagar.FirstOrDefaultAsync(x => x.Id == id);
                if (contaPagar is null)
                {
                    throw new Exception("Não foi possível retornar nenhuma conta a pagar!");
                }

                return contaPagar;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ContaPagar> Create([FromBody] ContaPagarDto data)
        {
            try
            {
                DateTime dataEmissao = Validacoes.DataEmissao(data.DataEmissao);

                if (data.DataVencimento.Date < dataEmissao)
                {
                    throw new Exception("A data do vencimento não pode ser menor que a data da emissão!");
                }

                if(data.DataPagamento.HasValue && data.DataPagamento.Value.Date < dataEmissao)
                {
                    throw new Exception("A data do pagamento não pode ser menor que a data da emissão!");
                }
             
                var contaPagarr = new ContaPagar
                    (data.Valor, data.TipoPagamento, dataEmissao, data.DataVencimento, data.DataPagamento, data.PedidoCompraId);

                _context.ContasPagar.Add(contaPagarr);
                await _context.SaveChangesAsync();

                return contaPagarr;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ContaPagar> Update(int id, [FromBody] ContaPagarUpdateDto data)
        {
            try
            {
                var contaPagar = await _context.ContasPagar.FirstOrDefaultAsync(x => x.Id == id);
                if (contaPagar is null)
                {
                    throw new Exception("Não foi possível retornar nenhuma conta a pagar!");
                }

                contaPagar.Valor = data.Valor;
                contaPagar.TipoPagamento = data.TipoPagamento;
                contaPagar.DataVencimento = data.DataVencimento;
                contaPagar.DataPagamento = data.DataPagamento;
                contaPagar.PedidoCompraId = data.PedidoCompraId;

                await _context.SaveChangesAsync();

                return contaPagar;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível atualizar a conta a pagar.", ex);
            }
        }

        public async Task<ContaPagar> Delete(int id)
        {
            try
            {
                var contaPagar = await _context.ContasPagar.FirstOrDefaultAsync(x => x.Id == id);
                if (contaPagar is null)
                {
                    throw new Exception("Não foi possível retornar nenhum pedido de compra!");
                }

                _context.ContasPagar.Remove(contaPagar);
                await _context.SaveChangesAsync();

                return contaPagar;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível deletar a conta a pagar.", ex);
            }
        }
    }
}

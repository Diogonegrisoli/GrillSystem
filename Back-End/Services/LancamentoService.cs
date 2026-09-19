using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class LancamentoService
    {
        private readonly AppDbContext _context;
        public LancamentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Lancamento>> ListAll()
        {
            try
            {
                var lancamento = await _context.Lancamentos.ToListAsync();
                if (lancamento is null)
                {
                    throw new Exception("Não foi possível retornar nenhum lançamento!");
                }

                return lancamento;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Lancamento> GetId(int id)
        {
            try
            {
                var categoria = await _context.Lancamentos.FirstOrDefaultAsync(x => x.Id == id);
                if (categoria is null)
                {
                    throw new Exception("Não foi possível retornar nenhum lançamento!");
                }

                return categoria;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Lancamento> Create([FromBody] LancamentoDto data)
        {
            try
            {
                var lancamento = new Lancamento
                    (data.Valor, data.Data, data.Descricao, data.CategoriaFinanceiraId, data.FuncionarioId);

                _context.Lancamentos.Add(lancamento);
                await _context.SaveChangesAsync();

                return lancamento;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Lancamento> Update(int id, [FromBody] LancamentoDto data)
        {
            try
            {
                var lancamento = await _context.Lancamentos.FirstOrDefaultAsync(x => x.Id == id);
                if (lancamento is null)
                {
                    throw new Exception("Não foi possível retornar nenhum lançamento!");
                }

                lancamento.Valor = data.Valor;
                lancamento.Data = data.Data;
                lancamento.Descricao = data.Descricao;
                lancamento.CategoriaFinanceiraId = data.CategoriaFinanceiraId;
                lancamento.FuncionarioId = data.FuncionarioId;

                await _context.SaveChangesAsync();

                return lancamento;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível atualizar o lançamento.", ex);
            }
        }

        public async Task<Lancamento> Delete(int id)
        {
            try
            {
                var lancamento = await _context.Lancamentos.FirstOrDefaultAsync(x => x.Id == id);
                if (lancamento is null)
                {
                    throw new Exception("Não foi possível retornar nenhum lançamento!");
                }

                _context.Lancamentos.Remove(lancamento);
                await _context.SaveChangesAsync();

                return lancamento;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível deletar o lançamento.", ex);
            }
        }
    }
}

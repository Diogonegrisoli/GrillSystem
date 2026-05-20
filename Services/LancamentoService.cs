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
            var lancamento = await _context.Lancamentos.ToListAsync();
            if (lancamento is null)
            {
                throw new Exception("Não foi possível retornar nenhum lançamento!");
            }

            return lancamento;
        }

        public async Task<Lancamento> GetId(int id)
        {
            var categoria = await _context.Lancamentos.FirstOrDefaultAsync(x => x.Id == id);
            if (categoria is null)
            {
                throw new Exception("Não foi possível retornar nenhum lançamento!");
            }

            return categoria;
        }

        public async Task<Lancamento> Create([FromBody] LancamentoDto data)
        {
            var lancamento = new Lancamento
                (data.Valor, data.Data, data.Descricao, data.CategoriaFinanceiraId, data.FuncionarioId);

            _context.Lancamentos.Add(lancamento);
            await _context.SaveChangesAsync();

            return lancamento;
        }

        public async Task<Lancamento> Update(int id, [FromBody] LancamentoDto data)
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

        public async Task Delete(int id)
        {
            var lancamento = await _context.Lancamentos.FirstOrDefaultAsync(x => x.Id == id);
            if (lancamento is null)
            {
                throw new Exception("Não foi possível retornar nenhum lançamento!");
            }

            _context.Lancamentos.Remove(lancamento);
            await _context.SaveChangesAsync();
        }
    }
}

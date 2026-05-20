using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class MovimentacaoEstoqueService
    {
        private readonly AppDbContext _context;
        public MovimentacaoEstoqueService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<MovimentacaoEstoque>> ListAll()
        {
            try
            {
                var movimentacao = await _context.MovimentacoesEstoque.ToListAsync();
                if (movimentacao is null)
                {
                    throw new Exception("Não foi possível retornar nenhuma movimentação do estoque!");
                }

                return movimentacao;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<MovimentacaoEstoque> GetId(int id)
        {
            try
            {
                var movimentacao = await _context.MovimentacoesEstoque.FirstOrDefaultAsync(x => x.Id == id);
                if (movimentacao is null)
                {
                    throw new Exception($"A movimentação do estoque com o id {id}# não foi localizada!");
                }
                return movimentacao;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<MovimentacaoEstoque> Create([FromBody] MovimentacaoEstoqueDto data)
        {
            var movimentacao = new MovimentacaoEstoque
            (data.Tipo, data.Quantidade, data.CustoUnitario, data.Data, data.Referencia, data.MateriaPrimaId);

            _context.MovimentacoesEstoque.Add(movimentacao);
            await _context.SaveChangesAsync();

            return movimentacao;
        }

        public async Task<MovimentacaoEstoque> Update(int id, [FromBody] MovimentacaoEstoqueDto data)
        {
            var movimentacao = await _context.MovimentacoesEstoque.FirstOrDefaultAsync(x => x.Id == id);
            if (movimentacao is null)
            {
                throw new Exception($"A movimentação no estoque com o id {id}# não foi localizado!");
            }

            movimentacao.Tipo = data.Tipo;
            movimentacao.Quantidade = data.Quantidade;
            movimentacao.CustoUnitario = data.CustoUnitario;
            movimentacao.Data = data.Data;
            movimentacao.Referencia = data.Referencia;
            movimentacao.MateriaPrimaId = data.MateriaPrimaId;

            await _context.SaveChangesAsync();


            return movimentacao;
        }

        public async Task<MovimentacaoEstoque> Delete(int id)
        {
            var movimentacao = await _context.MovimentacoesEstoque.FirstOrDefaultAsync(x => x.Id == id);
            if (movimentacao is null)
            {
                throw new Exception($"A movimentação no estoque com o id {id}# não foi localizado!");
            }

            _context.MovimentacoesEstoque.Remove(movimentacao);
            await _context.SaveChangesAsync();

            return movimentacao;
        }
    }
}

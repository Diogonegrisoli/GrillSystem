using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class ProdutoMateriaPrimaServices
    {
        private readonly AppDbContext _context;
        public ProdutoMateriaPrimaServices(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ICollection<ProdutoMateriaPrima>> ListAll()
        {
            try
            {
                var produto = await _context.ProdutosMateriasPrimas.ToListAsync();
                if (produto is null)
                {
                    throw new Exception("Não foi possível retornar nenhum produto/materia-prima!");
                }

                return produto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProdutoMateriaPrima> GetId(int id)
        {
            try
            {
                var produto = await _context.ProdutosMateriasPrimas.FirstOrDefaultAsync(x => x.Id == id);
                if (produto is null)
                {
                    throw new Exception($"O produto/materia-prima com o id {id}# não foi localizado!");
                }
                return produto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProdutoMateriaPrima> Create([FromBody] ProdutoMateriaPrimaDto data)
        {
            try
            {
                var produto = new ProdutoMateriaPrima
                (data.ProdutoId, data.MateriaPrimaId);

                _context.ProdutosMateriasPrimas.Add(produto);
                await _context.SaveChangesAsync();

                return produto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProdutoMateriaPrima> Update(int id, [FromBody] ProdutoMateriaPrimaDto data)
        {
            try
            {
                var produto = await _context.ProdutosMateriasPrimas.FirstOrDefaultAsync(x => x.Id == id);
                if (produto is null)
                {
                    throw new Exception($"O materia-prima com o id {id}# não foi localizado!");
                }

                produto.ProdutoId = data.ProdutoId;
                produto.MateriaPrimaId = data.MateriaPrimaId;

                await _context.SaveChangesAsync();


                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível atualizar o produto/matéria-prima.", ex);
            }
        }

        public async Task<ProdutoMateriaPrima> Delete(int id)
        {
            try
            {
                var materiaPrima = await _context.ProdutosMateriasPrimas.FirstOrDefaultAsync(x => x.Id == id);
                if (materiaPrima is null)
                {
                    throw new Exception($"A materia-prima com o id {id}# não foi localizado!");
                }

                _context.ProdutosMateriasPrimas.Remove(materiaPrima);
                await _context.SaveChangesAsync();

                return materiaPrima;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível deletar o produto/matéria-prima.", ex);
            }
        }
    }
}

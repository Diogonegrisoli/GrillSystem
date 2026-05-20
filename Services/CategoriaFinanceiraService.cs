using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class CategoriaFinanceiraService
    {
        private readonly AppDbContext _context;
        public CategoriaFinanceiraService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<CategoriaFinanceira>> ListAll()
        {
            var categoria = await _context.CategoriasFinanceiras.ToListAsync();
            if (categoria is null)
            {
                throw new Exception("Não foi possível retornar nenhuma categoria financeira!");
            }

            return categoria;
        }

        public async Task<CategoriaFinanceira> GetId(int id)
        {
            var categoria = await _context.CategoriasFinanceiras.FirstOrDefaultAsync(x => x.Id == id);
            if (categoria is null)
            {
                throw new Exception("Não foi possível retornar nenhuma categoria financeira!");
            }

            return categoria;
        }

        public async Task<CategoriaFinanceira> Create([FromBody] CategoriaFinanceiraDto data)
        {
            var categoria = new CategoriaFinanceira
                (data.Nome, data.Tipo);

            _context.CategoriasFinanceiras.Add(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }

        public async Task<CategoriaFinanceira> Update(int id, [FromBody] CategoriaFinanceiraDto data)
        {
            var categoria = await _context.CategoriasFinanceiras.FirstOrDefaultAsync(x => x.Id == id);
            if (categoria is null)
            {
                throw new Exception("Não foi possível retornar nenhuma categoria financeira!");
            }

            categoria.Nome = data.Nome;
            categoria.Tipo = data.Tipo;

            await _context.SaveChangesAsync();

            return categoria;
        }

        public async Task Delete(int id)
        {
            var categoria = await _context.CategoriasFinanceiras.FirstOrDefaultAsync(x => x.Id == id);
            if (categoria is null)
            {
                throw new Exception("Não foi possível retornar nenhuma categoria financeira!");
            }

            _context.CategoriasFinanceiras.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}

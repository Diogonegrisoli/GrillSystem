using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class MateriaPrimaServices
    {
        private readonly AppDbContext _context;
        public MateriaPrimaServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<MateriaPrima>> ListAll()
        {
            try
            {
                var materiaPrima = await _context.MateriasPrimas.ToListAsync();
                if (materiaPrima is null)
                {
                    throw new Exception("Não foi possível retornar nenhuma materia-prima!");
                }

                return materiaPrima;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<MateriaPrima> GetId(int id)
        {
            try
            {
                var materiaPrima = await _context.MateriasPrimas.FirstOrDefaultAsync(x => x.Id == id);
                if (materiaPrima is null)
                {
                    throw new Exception($"A materia-prima com o id {id}# não foi localizado!");
                }
                return materiaPrima;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<MateriaPrima> Create([FromBody] MateriaPrimaDto data)
        {
            var materiaPrima = new MateriaPrima
            (data.Codigo, data.Descricao, data.Quantidade, data.QuantidadeMinima, data.UnidadeMedida);

            _context.MateriasPrimas.Add(materiaPrima);
            await _context.SaveChangesAsync();

            return materiaPrima;
        }

        public async Task<MateriaPrima> Update(int id, [FromBody] MateriaPrimaDto data)
        {
            var materiaPrima = await _context.MateriasPrimas.FirstOrDefaultAsync(x => x.Id == id);
            if (materiaPrima is null)
            {
                throw new Exception($"O materia-prima com o id {id}# não foi localizado!");
            }

            materiaPrima.Codigo = data.Codigo;
            materiaPrima.Descricao  = data.Descricao;
            materiaPrima.Quantidade = data.QuantidadeMinima;
            materiaPrima.QuantidadeMinima = data.QuantidadeMinima;
            materiaPrima.UnidadeMedida = data.UnidadeMedida;

            await _context.SaveChangesAsync();


            return materiaPrima;
        }

        public async Task<MateriaPrima> Delete(int id)
        {
            var materiaPrima = await _context.MateriasPrimas.FirstOrDefaultAsync(x => x.Id == id);
            if (materiaPrima is null)
            {
                throw new Exception($"A materia-prima com o id {id}# não foi localizado!");
            }

            _context.MateriasPrimas.Remove(materiaPrima);
            await _context.SaveChangesAsync();

            return materiaPrima;
        }
    }
}

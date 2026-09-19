using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Services
{
    public class FornecedorMateriaPrimaServices
    {
        private readonly AppDbContext _context;
        public FornecedorMateriaPrimaServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<FornecedorMateriaPrima>> ListAll()
        {
            try
            {
                var fornecedorMateria = await _context.FornecedoresMateriaPrima.ToListAsync();
                if (fornecedorMateria is null)
                {
                    throw new Exception("Não foi possível retornar nenhum fornecedor/matéria-prima!");
                }

                return fornecedorMateria;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FornecedorMateriaPrima> GetId(int id)
        {
            try
            {
                var fornecedorMateria = await _context.FornecedoresMateriaPrima.FirstOrDefaultAsync(x => x.Id == id);
                if (fornecedorMateria is null)
                {
                    throw new Exception("Não foi possível retornar nenhum fornecedor/matéria-prima!");
                }

                return fornecedorMateria;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FornecedorMateriaPrima> Create([FromBody] FornecedorMateriaPrimaDto data)
        {
            try
            {
                var fornecedorMateria = new FornecedorMateriaPrima
                    (data.FornecedorId, data.MateriaPrimaId);

                _context.FornecedoresMateriaPrima.Add(fornecedorMateria);
                await _context.SaveChangesAsync();

                return fornecedorMateria;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FornecedorMateriaPrima> Update(int id, [FromBody] FornecedorMateriaPrimaDto data)
        {
            try
            {
                var fornecedorMateria = await _context.FornecedoresMateriaPrima.FirstOrDefaultAsync(x => x.Id == id);
                if (fornecedorMateria is null)
                {
                    throw new Exception("Não foi possível retornar nenhum fornecedor/matéria-prima!");
                }

                fornecedorMateria.FornecedorId = data.FornecedorId;
                fornecedorMateria.MateriaPrimaId = data.MateriaPrimaId;

                await _context.SaveChangesAsync();

                return fornecedorMateria;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível atualizar o fornecedor/matéria-prima.", ex);
            }
        }

        public async Task<FornecedorMateriaPrima> Delete(int id)
        {
            try
            {
                var fornecedorMateria = await _context.FornecedoresMateriaPrima.FirstOrDefaultAsync(x => x.Id == id);
                if (fornecedorMateria is null)
                {
                    throw new Exception("Não foi possível retornar nenhum fornecedor/matéria-prima!");
                }

                _context.FornecedoresMateriaPrima.Remove(fornecedorMateria);
                await _context.SaveChangesAsync();

                return fornecedorMateria;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível deletar o fornecedor/matéria-prima.", ex);
            }
        }
    }
}

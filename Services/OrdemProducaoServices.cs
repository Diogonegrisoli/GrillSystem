using GrillSystem.Data;
using GrillSystem.Dto;
using GrillSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace GrillSystem.Services
{
    public class OrdemProducaoServices
    {
        private readonly AppDbContext _context;

        public OrdemProducaoServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<OrdemProducao>> ListAll()
        {
            try
            {
                var ordens = await _context.OrdensProducao.ToListAsync();
                if (ordens is null)
                {
                    throw new Exception("Não foi encontrado nenhuma ordem de produção.");
                }

                return ordens;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<OrdemProducao> GetId(int id)
        {
            try
            {
                var ordem = await _context.OrdensProducao.FirstOrDefaultAsync(x => x.Id == id);
                if (ordem is null)
                {
                    throw new Exception($"A ordem de produção com o id {id}# não foi localizado.");
                }

                return ordem;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<ActionResult<OrdemProducao>> Create([FromBody] OrdemProducaoDto data)
        {
            try
            {
                var ordem = new OrdemProducao
                (data.Quantidade, data.DataInicio, data.DataFim, data.Tipo);

                _context.Add(ordem);
                await _context.SaveChangesAsync();

                return ordem;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<OrdemProducao> Update(int id, [FromBody] OrdemProducaoUpdateDto data)
        {
            try
            {
                var ordem = await _context.OrdensProducao.FirstOrDefaultAsync(x => x.Id == id);
                if (ordem is null)
                {
                    throw new Exception($"A ordem de produção com o id {id}# não foi localizado.");
                }

                ordem.Quantidade = data.Quantidade;
                ordem.DataInicio = data.DataInicio;
                ordem.DataFim = data.DataFim;
                ordem.Tipo = data.Tipo;
                ordem.Status = data.Status;

                await _context.SaveChangesAsync();

                return ordem;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }

        public async Task<OrdemProducao> Delete(int id)
        {

            try
            {
                var ordem = await _context.OrdensProducao.FirstOrDefaultAsync(x => x.Id == id);
                if (ordem is null)
                {
                    throw new Exception($"A ordem de produção com o id {id}# não foi localizado.");
                }

                _context.OrdensProducao.Remove(ordem);
                await _context.SaveChangesAsync();

                return ordem;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao executar a ação!");
            }
        }
    }
}

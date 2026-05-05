using GrillSystem.Data;
using GrillSystem.Models;
using Microsoft.EntityFrameworkCore;

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

                return ordens;
            }
            catch (Exception ex)
            {
                throw new Exception("");
            }
        }

        public asyncask<>
    }
}

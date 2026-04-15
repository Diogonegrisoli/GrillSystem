using Microsoft.EntityFrameworkCore;
using GrillSystem.Models;

namespace GrillSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
                        
        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}

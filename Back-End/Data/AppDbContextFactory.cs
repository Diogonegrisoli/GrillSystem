using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GrillSystem.Data;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "server=localhost;database=grillsystem;user=root;password=design-time";

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0)))
            .Options;

        return new AppDbContext(options);
    }
}

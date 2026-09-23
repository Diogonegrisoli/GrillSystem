using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GrillSystem.Data;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string[] envPaths =
        {
            Path.Combine(currentDirectory, ".env"),
            Path.Combine(currentDirectory, "Back-End", ".env")
        };

        string? envPath = envPaths.FirstOrDefault(File.Exists);
        if (envPath is not null)
        {
            Env.Load(envPath);
        }

        string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? throw new InvalidOperationException(
                "Configure ConnectionStrings__DefaultConnection no arquivo .env ou nas variáveis do sistema.");

        string mysqlVersionText = Environment.GetEnvironmentVariable("MYSQL_VERSION") ?? "8.0.0";
        if (!Version.TryParse(mysqlVersionText, out Version? mysqlVersion))
        {
            throw new InvalidOperationException(
                "MYSQL_VERSION deve usar o formato numérico, por exemplo: 8.0.0.");
        }

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, new MySqlServerVersion(mysqlVersion))
            .Options;

        return new AppDbContext(options);
    }
}

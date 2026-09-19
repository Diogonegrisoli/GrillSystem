using GrillSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Data
{
    public class AppDbContext : IdentityDbContext<Usuario, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<OrdemProducao> OrdensProducao { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ContaReceber> ContasReceber { get; set; }
        public DbSet<PedidoVenda> PedidosVenda { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProdutoPedidoVenda> ProdutosPedidosVenda { get; set; }
        public DbSet<ProdutoOrdemProducao> ProdutosOrdensProducao { get; set; }
        public DbSet<MateriaPrima> MateriasPrimas { get; set; }
        public DbSet<ProdutoMateriaPrima> ProdutosMateriasPrimas { get; set; }
        public DbSet<MovimentacaoEstoque> MovimentacoesEstoque { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<FornecedorMateriaPrima> FornecedoresMateriaPrima { get; set; }
        public DbSet<PedidoCompra> PedidosCompra { get; set; }
        public DbSet<PedidoCompraMateriaPrima> PedidosCompraMateriasPrimas { get; set; }
        public DbSet<ContaPagar> ContasPagar { get; set; }
        public DbSet<CategoriaFinanceira> CategoriasFinanceiras { get; set; }
        public DbSet<Lancamento> Lancamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.Property(x => x.Email).HasMaxLength(150);
                entity.Property(x => x.NormalizedEmail).HasMaxLength(150);
                entity.Property(x => x.UserName).HasMaxLength(150);
                entity.Property(x => x.NormalizedUserName).HasMaxLength(150);
                entity.Property(x => x.PasswordHash).HasColumnName("SenhaHash");
                entity.HasIndex(x => x.FuncionarioId).IsUnique();
                entity.HasOne(x => x.Funcionario)
                    .WithOne()
                    .HasForeignKey<Usuario>(x => x.FuncionarioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<IdentityRole<int>>(entity =>
            {
                entity.ToTable("Perfis");
                entity.HasData(
                    new IdentityRole<int>
                    {
                        Id = 1,
                        Name = "Administrador",
                        NormalizedName = "ADMINISTRADOR",
                        ConcurrencyStamp = "PERFIL-ADMINISTRADOR"
                    },
                    new IdentityRole<int>
                    {
                        Id = 2,
                        Name = "Gerente",
                        NormalizedName = "GERENTE",
                        ConcurrencyStamp = "PERFIL-GERENTE"
                    },
                    new IdentityRole<int>
                    {
                        Id = 3,
                        Name = "Operador",
                        NormalizedName = "OPERADOR",
                        ConcurrencyStamp = "PERFIL-OPERADOR"
                    });
            });
            builder.Entity<IdentityUserRole<int>>().ToTable("UsuariosPerfis");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UsuariosClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UsuariosLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("PerfisClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UsuariosTokens");
        }
    }
}

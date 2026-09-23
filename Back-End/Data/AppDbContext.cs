using GrillSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GrillSystem.Data
{
    public class AppDbContext : IdentityDbContext<Usuario, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<OrdemProducao> OrdensProducao => Set<OrdemProducao>();
        public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<ContaReceber> ContasReceber => Set<ContaReceber>();
        public DbSet<PedidoVenda> PedidosVenda => Set<PedidoVenda>();
        public DbSet<Produto> Produtos => Set<Produto>();
        public DbSet<ProdutoPedidoVenda> ProdutosPedidosVenda => Set<ProdutoPedidoVenda>();
        public DbSet<ProdutoOrdemProducao> ProdutosOrdensProducao => Set<ProdutoOrdemProducao>();
        public DbSet<MateriaPrima> MateriasPrimas => Set<MateriaPrima>();
        public DbSet<ProdutoMateriaPrima> ProdutosMateriasPrimas => Set<ProdutoMateriaPrima>();
        public DbSet<MovimentacaoEstoque> MovimentacoesEstoque => Set<MovimentacaoEstoque>();
        public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
        public DbSet<FornecedorMateriaPrima> FornecedoresMateriaPrima => Set<FornecedorMateriaPrima>();
        public DbSet<PedidoCompra> PedidosCompra => Set<PedidoCompra>();
        public DbSet<PedidoCompraMateriaPrima> PedidosCompraMateriasPrimas => Set<PedidoCompraMateriaPrima>();
        public DbSet<ContaPagar> ContasPagar => Set<ContaPagar>();
        public DbSet<CategoriaFinanceira> CategoriasFinanceiras => Set<CategoriaFinanceira>();
        public DbSet<Lancamento> Lancamentos => Set<Lancamento>();

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
                    },
                    new IdentityRole<int>
                    {
                        Id = 4,
                        Name = "Mestre de Produção",
                        NormalizedName = "MESTRE DE PRODUÇÃO",
                        ConcurrencyStamp = "PERFIL-MESTRE-PRODUCAO"
                    },
                    new IdentityRole<int>
                    {
                        Id = 5,
                        Name = "Financeiro",
                        NormalizedName = "FINANCEIRO",
                        ConcurrencyStamp = "PERFIL-FINANCEIRO"
                    },
                    new IdentityRole<int>
                    {
                        Id = 6,
                        Name = "Vendedor",
                        NormalizedName = "VENDEDOR",
                        ConcurrencyStamp = "PERFIL-VENDEDOR"
                    },
                    new IdentityRole<int>
                    {
                        Id = 7,
                        Name = "Comprador",
                        NormalizedName = "COMPRADOR",
                        ConcurrencyStamp = "PERFIL-COMPRADOR"
                    });
            });
            builder.Entity<IdentityUserRole<int>>().ToTable("UsuariosPerfis");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UsuariosClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UsuariosLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("PerfisClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UsuariosTokens");

            builder.Entity<Cliente>(entity =>
            {
                entity.HasIndex(x => x.CpfCnpj).IsUnique();
                entity.HasMany<PedidoVenda>()
                    .WithOne(x => x.Cliente)
                    .HasForeignKey(x => x.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Funcionario>(entity =>
            {
                entity.HasIndex(x => x.Cpf).IsUnique();
                entity.Property(x => x.Nome).HasMaxLength(100);
                entity.Property(x => x.Cpf).HasMaxLength(11);
            });

            builder.Entity<Fornecedor>(entity =>
            {
                entity.HasIndex(x => x.Cnpj).IsUnique();
                entity.Property(x => x.RazaoSocial).HasMaxLength(150);
                entity.Property(x => x.NomeFantasia).HasMaxLength(200);
                entity.Property(x => x.Cnpj).HasMaxLength(14);
                entity.Property(x => x.Email).HasMaxLength(250);
                entity.Property(x => x.Endereco).HasMaxLength(300);
            });

            builder.Entity<MateriaPrima>(entity =>
            {
                entity.HasIndex(x => x.Codigo).IsUnique();
                entity.Property(x => x.Codigo).HasMaxLength(50);
                entity.Property(x => x.Descricao).HasMaxLength(250);
                entity.Property(x => x.Quantidade).HasPrecision(18, 3);
                entity.Property(x => x.QuantidadeMinima).HasPrecision(18, 3);
            });

            builder.Entity<Produto>(entity =>
            {
                entity.HasIndex(x => x.Codigo).IsUnique();
                entity.Property(x => x.Codigo).HasMaxLength(50);
                entity.Property(x => x.Descricao).HasMaxLength(250);
                entity.Property(x => x.Preco).HasPrecision(18, 2);
            });

            builder.Entity<ProdutoMateriaPrima>(entity =>
            {
                entity.HasIndex(x => new { x.ProdutoId, x.MateriaPrimaId }).IsUnique();
                entity.Property(x => x.QuantidadeNecessaria).HasPrecision(18, 3);
                entity.HasOne(x => x.Produto).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.MateriaPrima).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<FornecedorMateriaPrima>(entity =>
            {
                entity.HasIndex(x => new { x.FornecedorId, x.MateriaPrimaId }).IsUnique();
                entity.HasOne(x => x.Fornecedor).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.MateriaPrima).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ProdutoPedidoVenda>(entity =>
            {
                entity.HasIndex(x => new { x.PedidoVendaId, x.ProdutoId }).IsUnique();
                entity.Property(x => x.PrecoUnitario).HasPrecision(18, 2);
                entity.HasOne(x => x.Produto).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.PedidoVenda).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PedidoCompraMateriaPrima>(entity =>
            {
                entity.HasIndex(x => new { x.PedidoCompraId, x.MateriaPrimaId }).IsUnique();
                entity.Property(x => x.Quantidade).HasPrecision(18, 3);
                entity.Property(x => x.CustoUnitario).HasPrecision(18, 2);
                entity.HasOne(x => x.MateriaPrima).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.PedidoCompra).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ProdutoOrdemProducao>(entity =>
            {
                entity.HasIndex(x => new { x.OrdemProducaoId, x.ProdutoId }).IsUnique();
                entity.HasIndex(x => x.OrdemProducaoId).IsUnique();
                entity.HasOne(x => x.Produto).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.OrdemProducao).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PedidoVenda>(entity =>
            {
                entity.Property(x => x.ValorTotal).HasPrecision(18, 2);
                entity.HasOne(x => x.Cliente).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<PedidoCompra>(entity =>
            {
                entity.Property(x => x.ValorTotal).HasPrecision(18, 2);
                entity.HasOne(x => x.Fornecedor).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.Funcionario).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ContaReceber>(entity =>
            {
                entity.HasIndex(x => x.PedidoVendaId).IsUnique();
                entity.Property(x => x.Valor).HasPrecision(18, 2);
                entity.HasOne(x => x.PedidoVenda).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ContaPagar>(entity =>
            {
                entity.HasIndex(x => x.PedidoCompraId).IsUnique();
                entity.Property(x => x.Valor).HasPrecision(18, 2);
                entity.HasOne(x => x.PedidoCompra).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<MovimentacaoEstoque>(entity =>
            {
                entity.Property(x => x.Quantidade).HasPrecision(18, 3);
                entity.Property(x => x.CustoUnitario).HasPrecision(18, 2);
                entity.Property(x => x.Referencia).HasMaxLength(200);
                entity.HasOne(x => x.MateriaPrima).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Lancamento>(entity =>
            {
                entity.Property(x => x.Valor).HasPrecision(18, 2);
                entity.Property(x => x.Descricao).HasMaxLength(300);
                entity.HasOne(x => x.CategoriaFinanceira).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.Funcionario).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<CategoriaFinanceira>(entity =>
                entity.Property(x => x.Nome).HasMaxLength(100));
        }
    }
}

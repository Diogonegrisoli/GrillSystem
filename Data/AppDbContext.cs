using Microsoft.EntityFrameworkCore;
using GrillSystem.Models;

namespace GrillSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

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
    }
}

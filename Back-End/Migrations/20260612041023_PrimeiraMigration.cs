using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrillSystem.Migrations
{
    /// <inheritdoc />
    public partial class PrimeiraMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategoriasFinanceiras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasFinanceiras", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cpf_cnpj = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo = table.Column<int>(type: "int", nullable: false),
                    telefone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    endereco = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomeFantasia = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RazaoSocial = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cnpj = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Endereco = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cpf = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MateriasPrimas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantidade = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    QuantidadeMinima = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    UnidadeMedida = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriasPrimas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrdensProducao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    DataFim = table.Column<DateOnly>(type: "date", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdensProducao", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Preco = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PedidosVenda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataPedido = table.Column<DateOnly>(type: "date", nullable: false),
                    DataEntrega = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosVenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidosVenda_cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Lancamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Valor = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    Descricao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoriaFinanceiraId = table.Column<int>(type: "int", nullable: false),
                    FuncionarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lancamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lancamentos_CategoriasFinanceiras_CategoriaFinanceiraId",
                        column: x => x.CategoriaFinanceiraId,
                        principalTable: "CategoriasFinanceiras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lancamentos_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PedidosCompra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataPedido = table.Column<DateOnly>(type: "date", nullable: false),
                    DataEntrega = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    FornecedorId = table.Column<int>(type: "int", nullable: false),
                    FuncionarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosCompra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidosCompra_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidosCompra_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SenhaHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FuncionarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FornecedoresMateriaPrima",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FornecedorId = table.Column<int>(type: "int", nullable: false),
                    MateriaPrimaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FornecedoresMateriaPrima", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FornecedoresMateriaPrima_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FornecedoresMateriaPrima_MateriasPrimas_MateriaPrimaId",
                        column: x => x.MateriaPrimaId,
                        principalTable: "MateriasPrimas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MovimentacoesEstoque",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CustoUnitario = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    Referencia = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MateriaPrimaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacoesEstoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimentacoesEstoque_MateriasPrimas_MateriaPrimaId",
                        column: x => x.MateriaPrimaId,
                        principalTable: "MateriasPrimas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProdutosMateriasPrimas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    MateriaPrimaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosMateriasPrimas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutosMateriasPrimas_MateriasPrimas_MateriaPrimaId",
                        column: x => x.MateriaPrimaId,
                        principalTable: "MateriasPrimas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutosMateriasPrimas_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProdutosOrdensProducao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    OrdemProducaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosOrdensProducao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutosOrdensProducao_OrdensProducao_OrdemProducaoId",
                        column: x => x.OrdemProducaoId,
                        principalTable: "OrdensProducao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutosOrdensProducao_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContasReceber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Valor = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DataVencimento = table.Column<DateOnly>(type: "date", nullable: false),
                    DataRecebimento = table.Column<DateOnly>(type: "date", nullable: true),
                    DataEmissao = table.Column<DateOnly>(type: "date", nullable: false),
                    TipoPagamento = table.Column<int>(type: "int", nullable: false),
                    StatusPagamento = table.Column<int>(type: "int", nullable: false),
                    PedidoVendaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasReceber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContasReceber_PedidosVenda_PedidoVendaId",
                        column: x => x.PedidoVendaId,
                        principalTable: "PedidosVenda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProdutosPedidosVenda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    PedidoVendaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosPedidosVenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutosPedidosVenda_PedidosVenda_PedidoVendaId",
                        column: x => x.PedidoVendaId,
                        principalTable: "PedidosVenda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutosPedidosVenda_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContasPagar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Valor = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TipoPagamento = table.Column<int>(type: "int", nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PedidoCompraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContasPagar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContasPagar_PedidosCompra_PedidoCompraId",
                        column: x => x.PedidoCompraId,
                        principalTable: "PedidosCompra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PedidosCompraMateriasPrimas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PedidoCompraId = table.Column<int>(type: "int", nullable: false),
                    MateriaPrimaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosCompraMateriasPrimas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidosCompraMateriasPrimas_MateriasPrimas_MateriaPrimaId",
                        column: x => x.MateriaPrimaId,
                        principalTable: "MateriasPrimas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidosCompraMateriasPrimas_PedidosCompra_PedidoCompraId",
                        column: x => x.PedidoCompraId,
                        principalTable: "PedidosCompra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ContasPagar_PedidoCompraId",
                table: "ContasPagar",
                column: "PedidoCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasReceber_PedidoVendaId",
                table: "ContasReceber",
                column: "PedidoVendaId");

            migrationBuilder.CreateIndex(
                name: "IX_FornecedoresMateriaPrima_FornecedorId",
                table: "FornecedoresMateriaPrima",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_FornecedoresMateriaPrima_MateriaPrimaId",
                table: "FornecedoresMateriaPrima",
                column: "MateriaPrimaId");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_CategoriaFinanceiraId",
                table: "Lancamentos",
                column: "CategoriaFinanceiraId");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_FuncionarioId",
                table: "Lancamentos",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesEstoque_MateriaPrimaId",
                table: "MovimentacoesEstoque",
                column: "MateriaPrimaId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosCompra_FornecedorId",
                table: "PedidosCompra",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosCompra_FuncionarioId",
                table: "PedidosCompra",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosCompraMateriasPrimas_MateriaPrimaId",
                table: "PedidosCompraMateriasPrimas",
                column: "MateriaPrimaId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosCompraMateriasPrimas_PedidoCompraId",
                table: "PedidosCompraMateriasPrimas",
                column: "PedidoCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosVenda_ClienteId",
                table: "PedidosVenda",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosMateriasPrimas_MateriaPrimaId",
                table: "ProdutosMateriasPrimas",
                column: "MateriaPrimaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosMateriasPrimas_ProdutoId",
                table: "ProdutosMateriasPrimas",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosOrdensProducao_OrdemProducaoId",
                table: "ProdutosOrdensProducao",
                column: "OrdemProducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosOrdensProducao_ProdutoId",
                table: "ProdutosOrdensProducao",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosPedidosVenda_PedidoVendaId",
                table: "ProdutosPedidosVenda",
                column: "PedidoVendaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosPedidosVenda_ProdutoId",
                table: "ProdutosPedidosVenda",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_FuncionarioId",
                table: "Usuarios",
                column: "FuncionarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContasPagar");

            migrationBuilder.DropTable(
                name: "ContasReceber");

            migrationBuilder.DropTable(
                name: "FornecedoresMateriaPrima");

            migrationBuilder.DropTable(
                name: "Lancamentos");

            migrationBuilder.DropTable(
                name: "MovimentacoesEstoque");

            migrationBuilder.DropTable(
                name: "PedidosCompraMateriasPrimas");

            migrationBuilder.DropTable(
                name: "ProdutosMateriasPrimas");

            migrationBuilder.DropTable(
                name: "ProdutosOrdensProducao");

            migrationBuilder.DropTable(
                name: "ProdutosPedidosVenda");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "CategoriasFinanceiras");

            migrationBuilder.DropTable(
                name: "PedidosCompra");

            migrationBuilder.DropTable(
                name: "MateriasPrimas");

            migrationBuilder.DropTable(
                name: "OrdensProducao");

            migrationBuilder.DropTable(
                name: "PedidosVenda");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Fornecedores");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "cliente");
        }
    }
}

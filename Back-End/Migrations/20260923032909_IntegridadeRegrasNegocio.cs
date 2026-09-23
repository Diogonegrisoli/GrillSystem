using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GrillSystem.Migrations
{
    /// <inheritdoc />
    public partial class IntegridadeRegrasNegocio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE TEMPORARY TABLE `_PreflightIntegridadeRegrasNegocio` (
                    `Origem` varchar(60) NOT NULL,
                    `Chave` char(64) NOT NULL,
                    PRIMARY KEY (`Origem`, `Chave`)
                );
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO `_PreflightIntegridadeRegrasNegocio` (`Origem`, `Chave`)
                SELECT 'ProdutoPedidoVenda', SHA2(CONCAT(`PedidoVendaId`, ':', `ProdutoId`), 256)
                FROM `ProdutosPedidosVenda`
                UNION ALL
                SELECT 'ProdutoOrdemProducao', SHA2(CAST(`OrdemProducaoId` AS char), 256)
                FROM `ProdutosOrdensProducao`
                UNION ALL
                SELECT 'ProdutoMateriaPrima', SHA2(CONCAT(`ProdutoId`, ':', `MateriaPrimaId`), 256)
                FROM `ProdutosMateriasPrimas`
                UNION ALL
                SELECT 'PedidoCompraMateriaPrima', SHA2(CONCAT(`PedidoCompraId`, ':', `MateriaPrimaId`), 256)
                FROM `PedidosCompraMateriasPrimas`
                UNION ALL
                SELECT 'FornecedorMateriaPrima', SHA2(CONCAT(`FornecedorId`, ':', `MateriaPrimaId`), 256)
                FROM `FornecedoresMateriaPrima`
                UNION ALL
                SELECT 'ContaReceberPedido', SHA2(CAST(`PedidoVendaId` AS char), 256)
                FROM `ContasReceber`
                UNION ALL
                SELECT 'ContaPagarPedido', SHA2(CAST(`PedidoCompraId` AS char), 256)
                FROM `ContasPagar`
                UNION ALL
                SELECT 'ProdutoCodigo', SHA2(LOWER(`Codigo`), 256)
                FROM `Produtos`
                UNION ALL
                SELECT 'MateriaPrimaCodigo', SHA2(LOWER(`Codigo`), 256)
                FROM `MateriasPrimas`
                UNION ALL
                SELECT 'FuncionarioCpf', SHA2(`Cpf`, 256)
                FROM `Funcionarios`
                UNION ALL
                SELECT 'FornecedorCnpj', SHA2(`Cnpj`, 256)
                FROM `Fornecedores`
                UNION ALL
                SELECT 'ClienteCpfCnpj', SHA2(`cpf_cnpj`, 256)
                FROM `cliente`;
                """);

            migrationBuilder.Sql(
                "DROP TEMPORARY TABLE `_PreflightIntegridadeRegrasNegocio`;");

            migrationBuilder.DropForeignKey(
                name: "FK_ContasPagar_PedidosCompra_PedidoCompraId",
                table: "ContasPagar");

            migrationBuilder.DropForeignKey(
                name: "FK_ContasReceber_PedidosVenda_PedidoVendaId",
                table: "ContasReceber");

            migrationBuilder.DropForeignKey(
                name: "FK_FornecedoresMateriaPrima_Fornecedores_FornecedorId",
                table: "FornecedoresMateriaPrima");

            migrationBuilder.DropForeignKey(
                name: "FK_FornecedoresMateriaPrima_MateriasPrimas_MateriaPrimaId",
                table: "FornecedoresMateriaPrima");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_CategoriasFinanceiras_CategoriaFinanceiraId",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Funcionarios_FuncionarioId",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesEstoque_MateriasPrimas_MateriaPrimaId",
                table: "MovimentacoesEstoque");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosCompra_Fornecedores_FornecedorId",
                table: "PedidosCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosCompra_Funcionarios_FuncionarioId",
                table: "PedidosCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosCompraMateriasPrimas_MateriasPrimas_MateriaPrimaId",
                table: "PedidosCompraMateriasPrimas");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosCompraMateriasPrimas_PedidosCompra_PedidoCompraId",
                table: "PedidosCompraMateriasPrimas");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosVenda_cliente_ClienteId",
                table: "PedidosVenda");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosMateriasPrimas_MateriasPrimas_MateriaPrimaId",
                table: "ProdutosMateriasPrimas");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosMateriasPrimas_Produtos_ProdutoId",
                table: "ProdutosMateriasPrimas");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosOrdensProducao_OrdensProducao_OrdemProducaoId",
                table: "ProdutosOrdensProducao");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosOrdensProducao_Produtos_ProdutoId",
                table: "ProdutosOrdensProducao");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosPedidosVenda_PedidosVenda_PedidoVendaId",
                table: "ProdutosPedidosVenda");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosPedidosVenda_Produtos_ProdutoId",
                table: "ProdutosPedidosVenda");

            migrationBuilder.DropIndex(
                name: "IX_ProdutosPedidosVenda_PedidoVendaId",
                table: "ProdutosPedidosVenda");

            migrationBuilder.DropIndex(
                name: "IX_ProdutosOrdensProducao_OrdemProducaoId",
                table: "ProdutosOrdensProducao");

            migrationBuilder.DropIndex(
                name: "IX_ProdutosMateriasPrimas_ProdutoId",
                table: "ProdutosMateriasPrimas");

            migrationBuilder.DropIndex(
                name: "IX_PedidosCompraMateriasPrimas_PedidoCompraId",
                table: "PedidosCompraMateriasPrimas");

            migrationBuilder.DropIndex(
                name: "IX_FornecedoresMateriaPrima_FornecedorId",
                table: "FornecedoresMateriaPrima");

            migrationBuilder.DropIndex(
                name: "IX_ContasReceber_PedidoVendaId",
                table: "ContasReceber");

            migrationBuilder.DropIndex(
                name: "IX_ContasPagar_PedidoCompraId",
                table: "ContasPagar");

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoUnitario",
                table: "ProdutosPedidosVenda",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QuantidadeNecessaria",
                table: "ProdutosMateriasPrimas",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(
                """
                UPDATE `ProdutosPedidosVenda` AS item
                INNER JOIN `Produtos` AS produto ON produto.`Id` = item.`ProdutoId`
                SET item.`PrecoUnitario` = produto.`Preco`
                WHERE item.`PrecoUnitario` = 0;
                """);

            migrationBuilder.Sql(
                """
                UPDATE `PedidosVenda` AS pedido
                LEFT JOIN (
                    SELECT item.`PedidoVendaId`, SUM(item.`Quantidade` * item.`PrecoUnitario`) AS total
                    FROM `ProdutosPedidosVenda` AS item
                    GROUP BY item.`PedidoVendaId`
                ) AS totais ON totais.`PedidoVendaId` = pedido.`Id`
                SET pedido.`ValorTotal` = COALESCE(totais.total, 0);
                """);

            migrationBuilder.Sql(
                """
                UPDATE `ContasReceber` AS conta
                INNER JOIN `PedidosVenda` AS pedido ON pedido.`Id` = conta.`PedidoVendaId`
                SET conta.`Valor` = pedido.`ValorTotal`;
                """);

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Produtos",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Produtos",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Produtos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorTotal",
                table: "PedidosVenda",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddColumn<decimal>(
                name: "CustoUnitario",
                table: "PedidosCompraMateriasPrimas",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantidade",
                table: "PedidosCompraMateriasPrimas",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorTotal",
                table: "PedidosCompra",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DataFim",
                table: "OrdensProducao",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Referencia",
                table: "MovimentacoesEstoque",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantidade",
                table: "MovimentacoesEstoque",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CustoUnitario",
                table: "MovimentacoesEstoque",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QuantidadeMinima",
                table: "MateriasPrimas",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantidade",
                table: "MateriasPrimas",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "MateriasPrimas",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "MateriasPrimas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "Lancamentos",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Lancamentos",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Funcionarios",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Funcionarios",
                type: "varchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "RazaoSocial",
                table: "Fornecedores",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "NomeFantasia",
                table: "Fornecedores",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Endereco",
                table: "Fornecedores",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Fornecedores",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Fornecedores",
                type: "varchar(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "ContasReceber",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "ContasPagar",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "CategoriasFinanceiras",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Perfis",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 4, "PERFIL-MESTRE-PRODUCAO", "Mestre de Produção", "MESTRE DE PRODUÇÃO" },
                    { 5, "PERFIL-FINANCEIRO", "Financeiro", "FINANCEIRO" },
                    { 6, "PERFIL-VENDEDOR", "Vendedor", "VENDEDOR" },
                    { 7, "PERFIL-COMPRADOR", "Comprador", "COMPRADOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosPedidosVenda_PedidoVendaId_ProdutoId",
                table: "ProdutosPedidosVenda",
                columns: new[] { "PedidoVendaId", "ProdutoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosOrdensProducao_OrdemProducaoId",
                table: "ProdutosOrdensProducao",
                column: "OrdemProducaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosOrdensProducao_OrdemProducaoId_ProdutoId",
                table: "ProdutosOrdensProducao",
                columns: new[] { "OrdemProducaoId", "ProdutoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosMateriasPrimas_ProdutoId_MateriaPrimaId",
                table: "ProdutosMateriasPrimas",
                columns: new[] { "ProdutoId", "MateriaPrimaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Codigo",
                table: "Produtos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PedidosCompraMateriasPrimas_PedidoCompraId_MateriaPrimaId",
                table: "PedidosCompraMateriasPrimas",
                columns: new[] { "PedidoCompraId", "MateriaPrimaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MateriasPrimas_Codigo",
                table: "MateriasPrimas",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_Cpf",
                table: "Funcionarios",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FornecedoresMateriaPrima_FornecedorId_MateriaPrimaId",
                table: "FornecedoresMateriaPrima",
                columns: new[] { "FornecedorId", "MateriaPrimaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_Cnpj",
                table: "Fornecedores",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContasReceber_PedidoVendaId",
                table: "ContasReceber",
                column: "PedidoVendaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContasPagar_PedidoCompraId",
                table: "ContasPagar",
                column: "PedidoCompraId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cliente_cpf_cnpj",
                table: "cliente",
                column: "cpf_cnpj",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ContasPagar_PedidosCompra_PedidoCompraId",
                table: "ContasPagar",
                column: "PedidoCompraId",
                principalTable: "PedidosCompra",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContasReceber_PedidosVenda_PedidoVendaId",
                table: "ContasReceber",
                column: "PedidoVendaId",
                principalTable: "PedidosVenda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FornecedoresMateriaPrima_Fornecedores_FornecedorId",
                table: "FornecedoresMateriaPrima",
                column: "FornecedorId",
                principalTable: "Fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FornecedoresMateriaPrima_MateriasPrimas_MateriaPrimaId",
                table: "FornecedoresMateriaPrima",
                column: "MateriaPrimaId",
                principalTable: "MateriasPrimas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_CategoriasFinanceiras_CategoriaFinanceiraId",
                table: "Lancamentos",
                column: "CategoriaFinanceiraId",
                principalTable: "CategoriasFinanceiras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Funcionarios_FuncionarioId",
                table: "Lancamentos",
                column: "FuncionarioId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesEstoque_MateriasPrimas_MateriaPrimaId",
                table: "MovimentacoesEstoque",
                column: "MateriaPrimaId",
                principalTable: "MateriasPrimas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosCompra_Fornecedores_FornecedorId",
                table: "PedidosCompra",
                column: "FornecedorId",
                principalTable: "Fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosCompra_Funcionarios_FuncionarioId",
                table: "PedidosCompra",
                column: "FuncionarioId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosCompraMateriasPrimas_MateriasPrimas_MateriaPrimaId",
                table: "PedidosCompraMateriasPrimas",
                column: "MateriaPrimaId",
                principalTable: "MateriasPrimas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosCompraMateriasPrimas_PedidosCompra_PedidoCompraId",
                table: "PedidosCompraMateriasPrimas",
                column: "PedidoCompraId",
                principalTable: "PedidosCompra",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosVenda_cliente_ClienteId",
                table: "PedidosVenda",
                column: "ClienteId",
                principalTable: "cliente",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosMateriasPrimas_MateriasPrimas_MateriaPrimaId",
                table: "ProdutosMateriasPrimas",
                column: "MateriaPrimaId",
                principalTable: "MateriasPrimas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosMateriasPrimas_Produtos_ProdutoId",
                table: "ProdutosMateriasPrimas",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosOrdensProducao_OrdensProducao_OrdemProducaoId",
                table: "ProdutosOrdensProducao",
                column: "OrdemProducaoId",
                principalTable: "OrdensProducao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosOrdensProducao_Produtos_ProdutoId",
                table: "ProdutosOrdensProducao",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosPedidosVenda_PedidosVenda_PedidoVendaId",
                table: "ProdutosPedidosVenda",
                column: "PedidoVendaId",
                principalTable: "PedidosVenda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosPedidosVenda_Produtos_ProdutoId",
                table: "ProdutosPedidosVenda",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContasPagar_PedidosCompra_PedidoCompraId",
                table: "ContasPagar");

            migrationBuilder.DropForeignKey(
                name: "FK_ContasReceber_PedidosVenda_PedidoVendaId",
                table: "ContasReceber");

            migrationBuilder.DropForeignKey(
                name: "FK_FornecedoresMateriaPrima_Fornecedores_FornecedorId",
                table: "FornecedoresMateriaPrima");

            migrationBuilder.DropForeignKey(
                name: "FK_FornecedoresMateriaPrima_MateriasPrimas_MateriaPrimaId",
                table: "FornecedoresMateriaPrima");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_CategoriasFinanceiras_CategoriaFinanceiraId",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Funcionarios_FuncionarioId",
                table: "Lancamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesEstoque_MateriasPrimas_MateriaPrimaId",
                table: "MovimentacoesEstoque");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosCompra_Fornecedores_FornecedorId",
                table: "PedidosCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosCompra_Funcionarios_FuncionarioId",
                table: "PedidosCompra");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosCompraMateriasPrimas_MateriasPrimas_MateriaPrimaId",
                table: "PedidosCompraMateriasPrimas");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosCompraMateriasPrimas_PedidosCompra_PedidoCompraId",
                table: "PedidosCompraMateriasPrimas");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosVenda_cliente_ClienteId",
                table: "PedidosVenda");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosMateriasPrimas_MateriasPrimas_MateriaPrimaId",
                table: "ProdutosMateriasPrimas");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosMateriasPrimas_Produtos_ProdutoId",
                table: "ProdutosMateriasPrimas");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosOrdensProducao_OrdensProducao_OrdemProducaoId",
                table: "ProdutosOrdensProducao");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosOrdensProducao_Produtos_ProdutoId",
                table: "ProdutosOrdensProducao");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosPedidosVenda_PedidosVenda_PedidoVendaId",
                table: "ProdutosPedidosVenda");

            migrationBuilder.DropForeignKey(
                name: "FK_ProdutosPedidosVenda_Produtos_ProdutoId",
                table: "ProdutosPedidosVenda");

            migrationBuilder.DropIndex(
                name: "IX_ProdutosPedidosVenda_PedidoVendaId_ProdutoId",
                table: "ProdutosPedidosVenda");

            migrationBuilder.DropIndex(
                name: "IX_ProdutosOrdensProducao_OrdemProducaoId",
                table: "ProdutosOrdensProducao");

            migrationBuilder.DropIndex(
                name: "IX_ProdutosOrdensProducao_OrdemProducaoId_ProdutoId",
                table: "ProdutosOrdensProducao");

            migrationBuilder.DropIndex(
                name: "IX_ProdutosMateriasPrimas_ProdutoId_MateriaPrimaId",
                table: "ProdutosMateriasPrimas");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_Codigo",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_PedidosCompraMateriasPrimas_PedidoCompraId_MateriaPrimaId",
                table: "PedidosCompraMateriasPrimas");

            migrationBuilder.DropIndex(
                name: "IX_MateriasPrimas_Codigo",
                table: "MateriasPrimas");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_Cpf",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_FornecedoresMateriaPrima_FornecedorId_MateriaPrimaId",
                table: "FornecedoresMateriaPrima");

            migrationBuilder.DropIndex(
                name: "IX_Fornecedores_Cnpj",
                table: "Fornecedores");

            migrationBuilder.DropIndex(
                name: "IX_ContasReceber_PedidoVendaId",
                table: "ContasReceber");

            migrationBuilder.DropIndex(
                name: "IX_ContasPagar_PedidoCompraId",
                table: "ContasPagar");

            migrationBuilder.DropIndex(
                name: "IX_cliente_cpf_cnpj",
                table: "cliente");

            migrationBuilder.DeleteData(
                table: "Perfis",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Perfis",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Perfis",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Perfis",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "PrecoUnitario",
                table: "ProdutosPedidosVenda");

            migrationBuilder.DropColumn(
                name: "QuantidadeNecessaria",
                table: "ProdutosMateriasPrimas");

            migrationBuilder.DropColumn(
                name: "CustoUnitario",
                table: "PedidosCompraMateriasPrimas");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "PedidosCompraMateriasPrimas");

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Produtos",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Produtos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "Produtos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorTotal",
                table: "PedidosVenda",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorTotal",
                table: "PedidosCompra",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DataFim",
                table: "OrdensProducao",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Referencia",
                table: "MovimentacoesEstoque",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantidade",
                table: "MovimentacoesEstoque",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldPrecision: 18,
                oldScale: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "CustoUnitario",
                table: "MovimentacoesEstoque",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "QuantidadeMinima",
                table: "MateriasPrimas",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldPrecision: 18,
                oldScale: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantidade",
                table: "MateriasPrimas",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldPrecision: 18,
                oldScale: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "MateriasPrimas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo",
                table: "MateriasPrimas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "Lancamentos",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Lancamentos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldMaxLength: 300)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Funcionarios",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Funcionarios",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldMaxLength: 11)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "RazaoSocial",
                table: "Fornecedores",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "NomeFantasia",
                table: "Fornecedores",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Endereco",
                table: "Fornecedores",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldMaxLength: 300)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Fornecedores",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldMaxLength: 250)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Fornecedores",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(14)",
                oldMaxLength: 14)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "ContasReceber",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "ContasPagar",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "CategoriasFinanceiras",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosPedidosVenda_PedidoVendaId",
                table: "ProdutosPedidosVenda",
                column: "PedidoVendaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosOrdensProducao_OrdemProducaoId",
                table: "ProdutosOrdensProducao",
                column: "OrdemProducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosMateriasPrimas_ProdutoId",
                table: "ProdutosMateriasPrimas",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosCompraMateriasPrimas_PedidoCompraId",
                table: "PedidosCompraMateriasPrimas",
                column: "PedidoCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_FornecedoresMateriaPrima_FornecedorId",
                table: "FornecedoresMateriaPrima",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasReceber_PedidoVendaId",
                table: "ContasReceber",
                column: "PedidoVendaId");

            migrationBuilder.CreateIndex(
                name: "IX_ContasPagar_PedidoCompraId",
                table: "ContasPagar",
                column: "PedidoCompraId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContasPagar_PedidosCompra_PedidoCompraId",
                table: "ContasPagar",
                column: "PedidoCompraId",
                principalTable: "PedidosCompra",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContasReceber_PedidosVenda_PedidoVendaId",
                table: "ContasReceber",
                column: "PedidoVendaId",
                principalTable: "PedidosVenda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FornecedoresMateriaPrima_Fornecedores_FornecedorId",
                table: "FornecedoresMateriaPrima",
                column: "FornecedorId",
                principalTable: "Fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FornecedoresMateriaPrima_MateriasPrimas_MateriaPrimaId",
                table: "FornecedoresMateriaPrima",
                column: "MateriaPrimaId",
                principalTable: "MateriasPrimas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_CategoriasFinanceiras_CategoriaFinanceiraId",
                table: "Lancamentos",
                column: "CategoriaFinanceiraId",
                principalTable: "CategoriasFinanceiras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Funcionarios_FuncionarioId",
                table: "Lancamentos",
                column: "FuncionarioId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesEstoque_MateriasPrimas_MateriaPrimaId",
                table: "MovimentacoesEstoque",
                column: "MateriaPrimaId",
                principalTable: "MateriasPrimas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosCompra_Fornecedores_FornecedorId",
                table: "PedidosCompra",
                column: "FornecedorId",
                principalTable: "Fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosCompra_Funcionarios_FuncionarioId",
                table: "PedidosCompra",
                column: "FuncionarioId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosCompraMateriasPrimas_MateriasPrimas_MateriaPrimaId",
                table: "PedidosCompraMateriasPrimas",
                column: "MateriaPrimaId",
                principalTable: "MateriasPrimas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosCompraMateriasPrimas_PedidosCompra_PedidoCompraId",
                table: "PedidosCompraMateriasPrimas",
                column: "PedidoCompraId",
                principalTable: "PedidosCompra",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosVenda_cliente_ClienteId",
                table: "PedidosVenda",
                column: "ClienteId",
                principalTable: "cliente",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosMateriasPrimas_MateriasPrimas_MateriaPrimaId",
                table: "ProdutosMateriasPrimas",
                column: "MateriaPrimaId",
                principalTable: "MateriasPrimas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosMateriasPrimas_Produtos_ProdutoId",
                table: "ProdutosMateriasPrimas",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosOrdensProducao_OrdensProducao_OrdemProducaoId",
                table: "ProdutosOrdensProducao",
                column: "OrdemProducaoId",
                principalTable: "OrdensProducao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosOrdensProducao_Produtos_ProdutoId",
                table: "ProdutosOrdensProducao",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosPedidosVenda_PedidosVenda_PedidoVendaId",
                table: "ProdutosPedidosVenda",
                column: "PedidoVendaId",
                principalTable: "PedidosVenda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutosPedidosVenda_Produtos_ProdutoId",
                table: "ProdutosPedidosVenda",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

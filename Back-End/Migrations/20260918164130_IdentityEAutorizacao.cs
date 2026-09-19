using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GrillSystem.Migrations
{
    /// <inheritdoc />
    public partial class IdentityEAutorizacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Funcionarios_FuncionarioId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_FuncionarioId",
                table: "Usuarios");

            migrationBuilder.AlterColumn<string>(
                name: "SenhaHash",
                table: "Usuarios",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Usuarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Usuarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Usuarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Usuarios",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Usuarios",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Usuarios",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Usuarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Usuarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Usuarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Usuarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Usuarios",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            // Compatibiliza os usuários existentes com os campos exigidos pelo Identity.
            migrationBuilder.Sql("""
                UPDATE `Usuarios`
                SET `UserName` = `Email`,
                    `NormalizedUserName` = UPPER(`Email`),
                    `NormalizedEmail` = UPPER(`Email`),
                    `EmailConfirmed` = 1,
                    `LockoutEnabled` = 1,
                    `SecurityStamp` = REPLACE(UUID(), '-', ''),
                    `ConcurrencyStamp` = REPLACE(UUID(), '-', '')
                """);

            migrationBuilder.CreateTable(
                name: "Perfis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfis", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsuariosClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosClaims_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsuariosLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UsuariosLogins_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsuariosTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UsuariosTokens_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PerfisClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfisClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfisClaims_Perfis_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsuariosPerfis",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosPerfis", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UsuariosPerfis_Perfis_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosPerfis_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Perfis",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "PERFIL-ADMINISTRADOR", "Administrador", "ADMINISTRADOR" },
                    { 2, "PERFIL-GERENTE", "Gerente", "GERENTE" },
                    { 3, "PERFIL-OPERADOR", "Operador", "OPERADOR" }
                });

            // O usuário mais antigo assume a administração; os demais começam como operadores.
            migrationBuilder.Sql("""
                INSERT INTO `UsuariosPerfis` (`UserId`, `RoleId`)
                SELECT `Id`,
                       CASE WHEN `Id` = COALESCE(
                           (SELECT `PrimeiroAtivoId` FROM (
                               SELECT MIN(`u2`.`Id`) AS `PrimeiroAtivoId`
                               FROM `Usuarios` AS `u2`
                               INNER JOIN `Funcionarios` AS `f2` ON `f2`.`Id` = `u2`.`FuncionarioId`
                               WHERE `f2`.`Status` = 0
                           ) AS `PrimeiroUsuarioAtivo`),
                           (SELECT `PrimeiroId` FROM (SELECT MIN(`Id`) AS `PrimeiroId` FROM `Usuarios`) AS `PrimeiroUsuario`))
                            THEN 1 ELSE 3 END
                FROM `Usuarios`
                """);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Usuarios",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_FuncionarioId",
                table: "Usuarios",
                column: "FuncionarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Usuarios",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Perfis",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerfisClaims_RoleId",
                table: "PerfisClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosClaims_UserId",
                table: "UsuariosClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosLogins_UserId",
                table: "UsuariosLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosPerfis_RoleId",
                table: "UsuariosPerfis",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Funcionarios_FuncionarioId",
                table: "Usuarios",
                column: "FuncionarioId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Funcionarios_FuncionarioId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "PerfisClaims");

            migrationBuilder.DropTable(
                name: "UsuariosClaims");

            migrationBuilder.DropTable(
                name: "UsuariosLogins");

            migrationBuilder.DropTable(
                name: "UsuariosPerfis");

            migrationBuilder.DropTable(
                name: "UsuariosTokens");

            migrationBuilder.DropTable(
                name: "Perfis");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_FuncionarioId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Usuarios");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "SenhaHash",
                keyValue: null,
                column: "SenhaHash",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "SenhaHash",
                table: "Usuarios",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Email",
                keyValue: null,
                column: "Email",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_FuncionarioId",
                table: "Usuarios",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Funcionarios_FuncionarioId",
                table: "Usuarios",
                column: "FuncionarioId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

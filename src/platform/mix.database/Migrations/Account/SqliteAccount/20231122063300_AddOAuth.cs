using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqliteAccount
{
    /// <inheritdoc />
    public partial class AddOAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_AspNetRolesId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_AspNetRolesId",
                table: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropIndex(
                name: "MixRoleNameIndex",
                table: "MixRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_AspNetRolesId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_AspNetRolesId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "AspNetRolesId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "AspNetRolesId",
                table: "AspNetRoleClaims");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "RefreshTokens",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "MixTenantId",
                table: "AspNetUserRoles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "OAuthClient",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    AllowedOrigin = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    ApplicationType = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    RefreshTokenLifeTime = table.Column<int>(type: "INTEGER", nullable: false),
                    Secret = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OAuthClient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OAuthToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "(newid())"),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    ClientId = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    SubjectId = table.Column<string>(type: "TEXT", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReferenceId = table.Column<string>(type: "TEXT", nullable: true),
                    TokenType = table.Column<string>(type: "TEXT", nullable: true),
                    TokenTypeHint = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Revoked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OAuthToken", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OAuthClient");

            migrationBuilder.DropTable(
                name: "OAuthToken");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "RefreshTokens",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "MixTenantId",
                table: "AspNetUserRoles",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "AspNetRolesId",
                table: "AspNetUserRoles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AspNetRolesId",
                table: "AspNetRoleClaims",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "(newid())"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    NormalizedName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    AllowedOrigin = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    ApplicationType = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    RefreshTokenLifeTime = table.Column<int>(type: "INTEGER", nullable: false),
                    Secret = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "MixRoleNameIndex",
                table: "MixRoles",
                column: "NormalizedName",
                filter: "(NormalizedName IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_AspNetRolesId",
                table: "AspNetUserRoles",
                column: "AspNetRolesId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_AspNetRolesId",
                table: "AspNetRoleClaims",
                column: "AspNetRolesId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "(NormalizedName IS NOT NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_AspNetRolesId",
                table: "AspNetRoleClaims",
                column: "AspNetRolesId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_AspNetRolesId",
                table: "AspNetUserRoles",
                column: "AspNetRolesId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }
    }
}

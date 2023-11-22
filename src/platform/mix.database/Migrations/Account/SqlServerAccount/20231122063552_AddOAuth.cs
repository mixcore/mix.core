using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqlServerAccount
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
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.CreateTable(
                name: "OAuthClient",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, collation: "Vietnamese_CI_AS"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    AllowedOrigin = table.Column<string>(type: "varchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    ApplicationType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    RefreshTokenLifeTime = table.Column<int>(type: "int", nullable: false),
                    Secret = table.Column<string>(type: "varchar(50)", nullable: false, collation: "Vietnamese_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OAuthClient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OAuthToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<string>(type: "varchar(50)", nullable: true, collation: "Vietnamese_CI_AS"),
                    SubjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenTypeHint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<bool>(type: "bit", nullable: false)
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
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<Guid>(
                name: "AspNetRolesId",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AspNetRolesId",
                table: "AspNetRoleClaims",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    NormalizedName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "Vietnamese_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, collation: "Vietnamese_CI_AS"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    AllowedOrigin = table.Column<string>(type: "varchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    ApplicationType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    RefreshTokenLifeTime = table.Column<int>(type: "int", nullable: false),
                    Secret = table.Column<string>(type: "varchar(50)", nullable: false, collation: "Vietnamese_CI_AS")
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

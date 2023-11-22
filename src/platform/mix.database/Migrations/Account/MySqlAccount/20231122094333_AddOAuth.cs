using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.MySqlAccount
{
    /// <inheritdoc />
    public partial class AddOAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OAuthClient",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowedOrigin = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ApplicationType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    RefreshTokenLifeTime = table.Column<int>(type: "int", nullable: false),
                    Secret = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OAuthClient", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OAuthToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(uuid())", collation: "ascii_general_ci"),
                    Token = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    SubjectId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReferenceId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TokenType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TokenTypeHint = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Revoked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OAuthToken", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OAuthClient");

            migrationBuilder.DropTable(
                name: "OAuthToken");
        }
    }
}

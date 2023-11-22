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
        }
    }
}

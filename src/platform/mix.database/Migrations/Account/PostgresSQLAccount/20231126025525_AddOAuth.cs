using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.PostgresSQLAccount
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                    ApplicationType = table.Column<string>(type: "varchar(50)", nullable: false),
                    RefreshTokenLifeTime = table.Column<int>(type: "integer", nullable: false),
                    Secret = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    UsePkce = table.Column<bool>(type: "boolean", nullable: false),
                    AllowedOrigins = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    GrantTypes = table.Column<string[]>(type: "text[]", nullable: true),
                    AllowedScopes = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    ClientUri = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    RedirectUris = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    AllowedProtectedResources = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OAuthClient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OAuthToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Token = table.Column<string>(type: "text", nullable: true),
                    ClientId = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    SubjectId = table.Column<string>(type: "text", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReferenceId = table.Column<string>(type: "text", nullable: true),
                    TokenType = table.Column<string>(type: "text", nullable: true),
                    TokenTypeHint = table.Column<string>(type: "text", nullable: true),
                    TokenStatus = table.Column<string>(type: "text", nullable: true),
                    Revoked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
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

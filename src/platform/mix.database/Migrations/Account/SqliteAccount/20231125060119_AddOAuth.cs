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
            migrationBuilder.CreateTable(
                name: "OAuthClient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    ApplicationType = table.Column<string>(type: "varchar(50)", nullable: false),
                    RefreshTokenLifeTime = table.Column<int>(type: "INTEGER", nullable: false),
                    Secret = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    UsePkce = table.Column<bool>(type: "INTEGER", nullable: false),
                    AllowedOrigins = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    GrantTypes = table.Column<string>(type: "TEXT", nullable: true),
                    AllowedScopes = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    ClientUri = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    RedirectUris = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    AllowedProtectedResources = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
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
                    TokenStatus = table.Column<string>(type: "TEXT", nullable: true),
                    Revoked = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
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

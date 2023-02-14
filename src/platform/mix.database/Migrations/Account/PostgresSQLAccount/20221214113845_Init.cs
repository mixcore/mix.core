using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mix.Database.Migrations.Account.PostgresSQLAccount
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    AllowedOrigin = table.Column<string>(type: "text", nullable: true),
                    ApplicationType = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenLifeTime = table.Column<int>(type: "integer", nullable: false),
                    Secret = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MixRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    NormalizedName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MixUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActived = table.Column<bool>(type: "boolean", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    RegisterType = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    LockoutEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    NormalizedUserName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Email = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    NormalizedEmail = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    SecurityStamp = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MixUserTenants",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "integer", nullable: false),
                    MixUserId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixUserTenants", x => new { x.MixUserId, x.TenantId });
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ClientId = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Email = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    Username = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    ExpiresUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IssuedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    MixRoleId = table.Column<Guid>(type: "uuid", nullable: true),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ClaimType = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    ClaimValue = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_MixRoles_MixRoleId",
                        column: x => x.MixRoleId,
                        principalTable: "MixRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MixUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    MixUserId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ClaimType = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    ClaimValue = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_MixUsers_MixUserId",
                        column: x => x.MixUserId,
                        principalTable: "MixUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_MixUsers_MixUserId1",
                        column: x => x.MixUserId1,
                        principalTable: "MixUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    ProviderKey = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    MixUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    MixUserId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins_1", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_MixUsers_MixUserId",
                        column: x => x.MixUserId,
                        principalTable: "MixUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_MixUsers_MixUserId1",
                        column: x => x.MixUserId1,
                        principalTable: "MixUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    MixTenantId = table.Column<int>(type: "int", nullable: false),
                    MixRoleId = table.Column<Guid>(type: "uuid", nullable: true),
                    MixUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    MixUserId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId, x.MixTenantId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_MixRoles_MixRoleId",
                        column: x => x.MixRoleId,
                        principalTable: "MixRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_MixUsers_MixUserId",
                        column: x => x.MixUserId,
                        principalTable: "MixUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_MixUsers_MixUserId1",
                        column: x => x.MixUserId1,
                        principalTable: "MixUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    LoginProvider = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false, collation: "und-x-icu"),
                    MixUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Value = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_MixUsers_MixUserId",
                        column: x => x.MixUserId,
                        principalTable: "MixUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_MixRoleId",
                table: "AspNetRoleClaims",
                column: "MixRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_MixUserId",
                table: "AspNetUserClaims",
                column: "MixUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_MixUserId1",
                table: "AspNetUserClaims",
                column: "MixUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_MixUserId",
                table: "AspNetUserLogins",
                column: "MixUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_MixUserId1",
                table: "AspNetUserLogins",
                column: "MixUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_MixRoleId",
                table: "AspNetUserRoles",
                column: "MixRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_MixUserId",
                table: "AspNetUserRoles",
                column: "MixUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_MixUserId1",
                table: "AspNetUserRoles",
                column: "MixUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserTokens_MixUserId",
                table: "AspNetUserTokens",
                column: "MixUserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "MixUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "MixUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "(\"NormalizedUserName\" IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_MixUserTenants_MixUserId",
                table: "MixUserTenants",
                column: "MixUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MixUserTenants_TenantId",
                table: "MixUserTenants",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "MixUserTenants");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "MixRoles");

            migrationBuilder.DropTable(
                name: "MixUsers");
        }
    }
}

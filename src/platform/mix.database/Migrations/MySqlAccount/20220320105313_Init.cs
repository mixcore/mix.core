using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.MySqlAccount
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "'uuid()'", collation: "ascii_general_ci"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NormalizedName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Clients",
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
                    table.PrimaryKey("PK_Clients", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MixRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "'uuid()'", collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NormalizedName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixRoles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MixUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "'uuid()'", collation: "ascii_general_ci"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActived = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    RegisterType = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    LockoutEnd = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NormalizedUserName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Email = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    NormalizedEmail = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    SecurityStamp = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixUsers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MixUserTenants",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MixUserId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "'uuid()'", collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixUserTenants", x => new { x.MixUserId, x.TenantId });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "'uuid()'", collation: "ascii_general_ci"),
                    ClientId = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Email = table.Column<string>(type: "varchar(250)", nullable: false, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Username = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ExpiresUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    IssuedUtc = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AspNetRolesId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MixRoleId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "'uuid()'", collation: "ascii_general_ci"),
                    ClaimType = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ClaimValue = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_AspNetRolesId",
                        column: x => x.AspNetRolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_MixRoles_MixRoleId",
                        column: x => x.MixRoleId,
                        principalTable: "MixRoles",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MixUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MixUserId1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "'uuid()'", collation: "ascii_general_ci"),
                    ClaimType = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ClaimValue = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    ProviderKey = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    MixUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MixUserId1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ProviderDisplayName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "'uuid()'", collation: "ascii_general_ci")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "'uuid()'", collation: "ascii_general_ci"),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "'uuid()'", collation: "ascii_general_ci"),
                    MixTenantId = table.Column<int>(type: "int", nullable: false),
                    AspNetRolesId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MixRoleId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MixUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    MixUserId1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId, x.MixTenantId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_AspNetRolesId",
                        column: x => x.AspNetRolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "'uuid()'", collation: "ascii_general_ci"),
                    LoginProvider = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    MixUserId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Value = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_MixUsers_MixUserId",
                        column: x => x.MixUserId,
                        principalTable: "MixUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_AspNetRolesId",
                table: "AspNetRoleClaims",
                column: "AspNetRolesId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_MixRoleId",
                table: "AspNetRoleClaims",
                column: "MixRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "(NormalizedName IS NOT NULL)");

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
                name: "IX_AspNetUserRoles_AspNetRolesId",
                table: "AspNetUserRoles",
                column: "AspNetRolesId");

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
                name: "MixRoleNameIndex",
                table: "MixRoles",
                column: "NormalizedName",
                filter: "(NormalizedName IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "MixUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "MixUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "(NormalizedUserName IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_MixUserTenants_MixUserId",
                table: "MixUserTenants",
                column: "MixUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MixUserTenants_TenantId",
                table: "MixUserTenants",
                column: "TenantId");
        }

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
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "MixRoles");

            migrationBuilder.DropTable(
                name: "MixUsers");
        }
    }
}

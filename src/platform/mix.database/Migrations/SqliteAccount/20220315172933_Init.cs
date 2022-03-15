using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqliteAccount
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
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

            migrationBuilder.CreateTable(
                name: "MixRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    MixTenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    NormalizedName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MixUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    JoinDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActived = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    RegisterType = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    Avatar = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    NickName = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    Gender = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Culture = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    DOB = table.Column<DateTime>(type: "datetime", nullable: true),
                    LockoutEnd = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    NormalizedUserName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    Email = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    NormalizedEmail = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    SecurityStamp = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MixUserTenants",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    MixUserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixUserTenants", x => new { x.MixUserId, x.TenantId });
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    ClientId = table.Column<Guid>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    Email = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    Username = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    ExpiresUtc = table.Column<DateTime>(type: "datetime", nullable: false),
                    IssuedUtc = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    AspNetRolesId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MixRoleId = table.Column<Guid>(type: "TEXT", nullable: true),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    ClaimType = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    ClaimValue = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE")
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MixUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MixUserId1 = table.Column<Guid>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    ClaimType = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    ClaimValue = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE")
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
                    LoginProvider = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    ProviderKey = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    MixUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MixUserId1 = table.Column<Guid>(type: "TEXT", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()")
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
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    AspNetRolesId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MixRoleId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MixUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MixUserId1 = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    LoginProvider = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    MixUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE")
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

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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    Id = table.Column<string>(type: "varchar(50)", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    AllowedOrigin = table.Column<string>(type: "varchar(250)", nullable: true),
                    ApplicationType = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "varchar(250)", nullable: false),
                    RefreshTokenLifeTime = table.Column<int>(type: "INTEGER", nullable: false),
                    Secret = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MixRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActived = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    RegisterType = table.Column<string>(type: "varchar(50)", nullable: true),
                    Avatar = table.Column<string>(type: "varchar(250)", nullable: true),
                    NickName = table.Column<string>(type: "varchar(50)", nullable: true),
                    FirstName = table.Column<string>(type: "varchar(50)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", nullable: true),
                    Gender = table.Column<string>(type: "varchar(50)", nullable: true),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Culture = table.Column<string>(type: "varchar(50)", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserName = table.Column<string>(type: "varchar(250)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(250)", nullable: true),
                    Email = table.Column<string>(type: "varchar(250)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(250)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(250)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "varchar(50)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(250)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
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
                    MixUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixUserTenants", x => new { x.MixUserId, x.TenantId });
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", maxLength: 50, nullable: false),
                    ClientId = table.Column<Guid>(type: "varchar(50)", nullable: false),
                    Email = table.Column<string>(type: "varchar(250)", nullable: false),
                    Username = table.Column<string>(type: "varchar(250)", nullable: true),
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
                    AspNetRolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MixRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoleId = table.Column<Guid>(type: "varchar(50)", nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(250)", nullable: true),
                    ClaimValue = table.Column<string>(type: "varchar(250)", nullable: true)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    MixUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MixUserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(250)", nullable: true),
                    ClaimValue = table.Column<string>(type: "varchar(250)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "varchar(50)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(50)", nullable: false),
                    MixUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MixUserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "varchar(250)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AspNetRolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MixRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MixUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MixUserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(50)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    MixUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Value = table.Column<string>(type: "varchar(4000)", nullable: true)
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
                unique: true,
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

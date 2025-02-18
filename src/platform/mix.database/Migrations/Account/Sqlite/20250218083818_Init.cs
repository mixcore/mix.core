using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Account.Sqlite
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "asp_net_role_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    role_id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    claim_type = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    claim_value = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asp_net_role_claims", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_user_login",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    provider_key = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    provider_display_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins_1", x => new { x.login_provider, x.provider_key });
                });

            migrationBuilder.CreateTable(
                name: "asp_net_user_roles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    role_id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asp_net_user_roles", x => new { x.user_id, x.role_id, x.tenant_id });
                });

            migrationBuilder.CreateTable(
                name: "asp_net_user_tokens",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    login_provider = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    name = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    value = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                });

            migrationBuilder.CreateTable(
                name: "mix_permission",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tenant_id = table.Column<int>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "TEXT", nullable: true),
                    group = table.Column<string>(type: "TEXT", nullable: true),
                    key = table.Column<string>(type: "TEXT", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_permission", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mix_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    normalized_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    concurrency_stamp = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mix_user_tenant",
                columns: table => new
                {
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    mix_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_user_tenant", x => new { x.mix_user_id, x.tenant_id });
                });

            migrationBuilder.CreateTable(
                name: "mix_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    register_type = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    lockout_end = table.Column<DateTime>(type: "datetime", nullable: true),
                    user_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    normalized_user_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    email = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    normalized_email = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    email_confirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    password_hash = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    security_stamp = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    concurrency_stamp = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    phone_number = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    phone_number_confirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    lockout_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "o_auth_client",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    application_type = table.Column<string>(type: "varchar(50)", nullable: false),
                    refresh_token_life_time = table.Column<int>(type: "INTEGER", nullable: false),
                    secret = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false),
                    use_pkce = table.Column<bool>(type: "INTEGER", nullable: false),
                    allowed_origins = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    grant_types = table.Column<string>(type: "text", nullable: true),
                    allowed_scopes = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    client_uri = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    redirect_uris = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    allowed_protected_resources = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_o_auth_client", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "o_auth_token",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    token = table.Column<string>(type: "TEXT", nullable: true),
                    client_id = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    subject_id = table.Column<string>(type: "TEXT", nullable: true),
                    creation_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    reference_id = table.Column<string>(type: "TEXT", nullable: true),
                    token_type = table.Column<string>(type: "TEXT", nullable: true),
                    token_type_hint = table.Column<string>(type: "TEXT", nullable: true),
                    token_status = table.Column<string>(type: "TEXT", nullable: true),
                    revoked = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_o_auth_token", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    client_id = table.Column<Guid>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    email = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    user_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    expired_utc = table.Column<DateTime>(type: "datetime", nullable: false),
                    issued_utc = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_mix_database_association",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tenant_id = table.Column<int>(type: "INTEGER", nullable: false),
                    parent_database_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    child_database_name = table.Column<string>(type: "varchar(250)", nullable: true),
                    guid_parent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    guid_child_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    parent_id = table.Column<int>(type: "INTEGER", nullable: false),
                    child_id = table.Column<int>(type: "INTEGER", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sys_mix_database_association", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_user_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    mix_user_id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    claim_type = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    claim_value = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asp_net_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "FK_asp_net_user_claims_mix_users_mix_user_id",
                        column: x => x.mix_user_id,
                        principalTable: "mix_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_asp_net_user_claims_mix_user_id",
                table: "asp_net_user_claims",
                column: "mix_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_asp_net_user_claims_user_id",
                table: "asp_net_user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_asp_net_user_login_user_id",
                table: "asp_net_user_login",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_asp_net_user_roles_role_id",
                table: "asp_net_user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_user_tenant_mix_user_id",
                table: "mix_user_tenant",
                column: "mix_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_user_tenant_tenant_id",
                table: "mix_user_tenant",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "email_index",
                table: "mix_users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "user_name_index",
                table: "mix_users",
                column: "normalized_user_name",
                unique: true,
                filter: "(normalized_user_name IS NOT NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asp_net_role_claims");

            migrationBuilder.DropTable(
                name: "asp_net_user_claims");

            migrationBuilder.DropTable(
                name: "asp_net_user_login");

            migrationBuilder.DropTable(
                name: "asp_net_user_roles");

            migrationBuilder.DropTable(
                name: "asp_net_user_tokens");

            migrationBuilder.DropTable(
                name: "mix_permission");

            migrationBuilder.DropTable(
                name: "mix_roles");

            migrationBuilder.DropTable(
                name: "mix_user_tenant");

            migrationBuilder.DropTable(
                name: "o_auth_client");

            migrationBuilder.DropTable(
                name: "o_auth_token");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "sys_mix_database_association");

            migrationBuilder.DropTable(
                name: "mix_users");
        }
    }
}

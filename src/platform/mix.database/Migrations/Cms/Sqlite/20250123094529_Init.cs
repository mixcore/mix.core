using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Cms.Sqlite
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mix_contributor",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    use_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_owner = table.Column<bool>(type: "INTEGER", nullable: false),
                    int_content_id = table.Column<int>(type: "integer", nullable: true),
                    guid_content_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    content_type = table.Column<string>(type: "varchar(50)", nullable: false),
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
                    table.PrimaryKey("pk_mix_contributor", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mix_database_association",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    parent_database_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    child_database_name = table.Column<string>(type: "varchar(250)", nullable: true),
                    guid_parent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    guid_child_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    parent_id = table.Column<int>(type: "integer", nullable: false),
                    child_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("pk_mix_database_association", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mix_discussion",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    use_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    int_content_id = table.Column<int>(type: "integer", nullable: true),
                    guid_content_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    content_type = table.Column<string>(type: "varchar(50)", nullable: false),
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
                    table.PrimaryKey("pk_mix_discussion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_module_association",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    mix_page_content_id = table.Column<int>(type: "integer", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    tenant_id = table.Column<int>(type: "INTEGER", nullable: false),
                    parent_id = table.Column<int>(type: "INTEGER", nullable: false),
                    child_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_page_module_association", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_post_association",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    mix_page_content_id = table.Column<int>(type: "integer", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    tenant_id = table.Column<int>(type: "INTEGER", nullable: false),
                    parent_id = table.Column<int>(type: "INTEGER", nullable: false),
                    child_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_page_post_association", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mix_post_post_association",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    tenant_id = table.Column<int>(type: "INTEGER", nullable: false),
                    parent_id = table.Column<int>(type: "INTEGER", nullable: false),
                    child_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_post_post_association", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mix_tenant",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    primary_domain = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    system_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
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
                    table.PrimaryKey("pk_mix_tenant", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mix_application",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    base_href = table.Column<string>(type: "varchar(250)", nullable: true),
                    deploy_url = table.Column<string>(type: "varchar(250)", nullable: true),
                    app_settings = table.Column<string>(type: "text", nullable: true),
                    domain = table.Column<string>(type: "varchar(250)", nullable: true),
                    base_api_url = table.Column<string>(type: "varchar(250)", nullable: true),
                    template_id = table.Column<int>(type: "integer", nullable: true),
                    image = table.Column<string>(type: "varchar(250)", nullable: true),
                    mix_database_name = table.Column<string>(type: "varchar(250)", nullable: true),
                    mix_db_id = table.Column<int>(type: "integer", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_application", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_application_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_configuration",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    system_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_configuration", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_configuration_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_culture",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    alias = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    icon = table.Column<string>(type: "varchar(4000)", nullable: true),
                    lcid = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    specificulture = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_culture", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_culture_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_database",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    mix_database_context_id = table.Column<int>(type: "INTEGER", nullable: true),
                    system_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    type = table.Column<string>(type: "varchar(50)", nullable: false),
                    read_permissions = table.Column<string>(type: "varchar(250)", nullable: true),
                    create_permissions = table.Column<string>(type: "varchar(250)", nullable: true),
                    update_permissions = table.Column<string>(type: "varchar(250)", nullable: true),
                    delete_permissions = table.Column<string>(type: "varchar(250)", nullable: true),
                    self_managed = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_database", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_database_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_database_context",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    database_provider = table.Column<string>(type: "varchar(50)", nullable: false),
                    aes_key = table.Column<string>(type: "varchar(250)", nullable: false),
                    connection_string = table.Column<string>(type: "varchar(250)", nullable: false),
                    schema = table.Column<string>(type: "varchar(50)", nullable: false),
                    naming_convention = table.Column<string>(type: "varchar(50)", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    system_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_database_context", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_database_context_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_domain",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    host = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_domain", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_domain_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_language",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    system_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_language", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_language_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "newid()"),
                    extension = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    file_folder = table.Column<string>(type: "varchar(250)", nullable: true),
                    file_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    file_properties = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    file_size = table.Column<long>(type: "INTEGER", nullable: false),
                    file_type = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    tags = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    target_url = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_media", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_media_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_module",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    type = table.Column<string>(type: "varchar(50)", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    system_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_module", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_module_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_page",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_page", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_page_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_post",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(4000)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_post", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_post_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_theme",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    image_url = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    preview_url = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    asset_folder = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    template_folder = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    mix_database_name = table.Column<string>(type: "varchar(50)", nullable: true),
                    mix_db_id = table.Column<int>(type: "INTEGER", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    system_name = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_theme", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_theme_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_url_alias",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    source_content_id = table.Column<int>(type: "integer", nullable: true),
                    source_content_guid_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    alias = table.Column<string>(type: "varchar(50)", nullable: true),
                    type = table.Column<string>(type: "varchar(50)", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_url_alias", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_url_alias_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_configuration_content",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    mix_configuration_id = table.Column<int>(type: "integer", nullable: true),
                    default_content = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    category = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    data_type = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    icon = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    is_public = table.Column<bool>(type: "INTEGER", nullable: false),
                    parent_id = table.Column<int>(type: "INTEGER", nullable: false),
                    mix_culture_id = table.Column<int>(type: "integer", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    system_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    content = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_configuration_content", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_configuration_content_mix_configuration_mix_configuration_id",
                        column: x => x.mix_configuration_id,
                        principalTable: "mix_configuration",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_mix_configuration_content_mix_culture_mix_culture_id",
                        column: x => x.mix_culture_id,
                        principalTable: "mix_culture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_database_column",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    system_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    mix_database_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    data_type = table.Column<string>(type: "varchar(50)", nullable: false),
                    configurations = table.Column<string>(type: "varchar(4000)", nullable: true),
                    reference_id = table.Column<int>(type: "integer", nullable: true),
                    default_value = table.Column<string>(type: "text", nullable: true, collation: "NOCASE"),
                    mix_database_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("pk_mix_database_column", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_database_column_mix_database_mix_database_id",
                        column: x => x.mix_database_id,
                        principalTable: "mix_database",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_database_relationship",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    parent_id = table.Column<int>(type: "INTEGER", nullable: false),
                    child_id = table.Column<int>(type: "INTEGER", nullable: false),
                    display_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    source_database_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    destinate_database_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    type = table.Column<string>(type: "varchar(50)", nullable: false),
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
                    table.PrimaryKey("pk_mix_database_relationship", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_database_relationship_mix_database_child_id",
                        column: x => x.child_id,
                        principalTable: "mix_database",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_mix_database_relationship_mix_database_parent_id",
                        column: x => x.parent_id,
                        principalTable: "mix_database",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "mix_language_content",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    default_content = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    category = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    data_type = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    mix_language_id = table.Column<int>(type: "integer", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    icon = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    is_public = table.Column<bool>(type: "INTEGER", nullable: false),
                    parent_id = table.Column<int>(type: "INTEGER", nullable: false),
                    mix_culture_id = table.Column<int>(type: "integer", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    system_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    content = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_language_content", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_language_content_mix_culture_mix_culture_id",
                        column: x => x.mix_culture_id,
                        principalTable: "mix_culture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mix_language_content_mix_language_mix_language_id",
                        column: x => x.mix_language_id,
                        principalTable: "mix_language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_module_content",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    mix_module_id = table.Column<int>(type: "INTEGER", nullable: false),
                    system_name = table.Column<string>(type: "varchar(250)", nullable: true),
                    class_name = table.Column<string>(type: "varchar(50)", nullable: true),
                    page_size = table.Column<int>(type: "INTEGER", nullable: true),
                    type = table.Column<string>(type: "varchar(50)", nullable: false),
                    simple_data_columns = table.Column<string>(type: "text", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    icon = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    is_public = table.Column<bool>(type: "INTEGER", nullable: false),
                    parent_id = table.Column<int>(type: "INTEGER", nullable: false),
                    mix_culture_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    excerpt = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    content = table.Column<string>(type: "text", nullable: true, collation: "NOCASE"),
                    layout_id = table.Column<int>(type: "integer", nullable: true),
                    template_id = table.Column<int>(type: "integer", nullable: true),
                    image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    seo_description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    seo_keywords = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    seo_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    seo_title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    published_date_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    mix_database_name = table.Column<string>(type: "varchar(50)", nullable: true),
                    mix_db_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_module_content", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_module_content_mix_culture_mix_culture_id",
                        column: x => x.mix_culture_id,
                        principalTable: "mix_culture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mix_module_content_mix_module_mix_module_id",
                        column: x => x.mix_module_id,
                        principalTable: "mix_module",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_content",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    class_name = table.Column<string>(type: "varchar(50)", nullable: true),
                    page_size = table.Column<int>(type: "integer", nullable: true),
                    type = table.Column<string>(type: "varchar(50)", nullable: false),
                    mix_page_id = table.Column<int>(type: "integer", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    icon = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    is_public = table.Column<bool>(type: "INTEGER", nullable: false),
                    parent_id = table.Column<int>(type: "INTEGER", nullable: false),
                    mix_culture_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    excerpt = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    content = table.Column<string>(type: "text", nullable: true, collation: "NOCASE"),
                    layout_id = table.Column<int>(type: "integer", nullable: true),
                    template_id = table.Column<int>(type: "integer", nullable: true),
                    image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    seo_description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    seo_keywords = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    seo_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    seo_title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    published_date_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    mix_database_name = table.Column<string>(type: "varchar(50)", nullable: true),
                    mix_db_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_page_content", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_page_content_mix_culture_mix_culture_id",
                        column: x => x.mix_culture_id,
                        principalTable: "mix_culture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mix_page_content_mix_page_mix_page_id",
                        column: x => x.mix_page_id,
                        principalTable: "mix_page",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_post_content",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    class_name = table.Column<string>(type: "varchar(50)", nullable: true),
                    mix_post_id = table.Column<int>(type: "integer", nullable: false),
                    mix_post_content_id = table.Column<int>(type: "integer", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    icon = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    is_public = table.Column<bool>(type: "INTEGER", nullable: false),
                    parent_id = table.Column<int>(type: "INTEGER", nullable: false),
                    mix_culture_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    excerpt = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    content = table.Column<string>(type: "text", nullable: true, collation: "NOCASE"),
                    layout_id = table.Column<int>(type: "integer", nullable: true),
                    template_id = table.Column<int>(type: "integer", nullable: true),
                    image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    seo_description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    seo_keywords = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    seo_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    seo_title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    published_date_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    mix_database_name = table.Column<string>(type: "varchar(50)", nullable: true),
                    mix_db_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_post_content", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_post_content_mix_culture_mix_culture_id",
                        column: x => x.mix_culture_id,
                        principalTable: "mix_culture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mix_post_content_mix_post_content_mix_post_content_id",
                        column: x => x.mix_post_content_id,
                        principalTable: "mix_post_content",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_mix_post_content_mix_post_mix_post_id",
                        column: x => x.mix_post_id,
                        principalTable: "mix_post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_template",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true, collation: "NOCASE"),
                    extension = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    file_folder = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    file_name = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    folder_type = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    scripts = table.Column<string>(type: "text", nullable: false, collation: "NOCASE"),
                    styles = table.Column<string>(type: "text", nullable: false, collation: "NOCASE"),
                    mix_theme_name = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    mix_theme_id = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("pk_mix_template", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_template_mix_theme_mix_theme_id",
                        column: x => x.mix_theme_id,
                        principalTable: "mix_theme",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_module_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    simple_data_columns = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    value = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    mix_module_content_id = table.Column<int>(type: "INTEGER", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false),
                    specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    icon = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    is_public = table.Column<bool>(type: "INTEGER", nullable: false),
                    parent_id = table.Column<int>(type: "INTEGER", nullable: false),
                    mix_culture_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    excerpt = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    content = table.Column<string>(type: "text", nullable: true, collation: "NOCASE"),
                    layout_id = table.Column<int>(type: "integer", nullable: true),
                    template_id = table.Column<int>(type: "integer", nullable: true),
                    image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    seo_description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    seo_keywords = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    seo_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    seo_title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    published_date_time = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_module_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_module_data_mix_culture_mix_culture_id",
                        column: x => x.mix_culture_id,
                        principalTable: "mix_culture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mix_module_data_mix_module_content_mix_module_content_id",
                        column: x => x.mix_module_content_id,
                        principalTable: "mix_module_content",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_module_post_association",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    mix_module_content_id = table.Column<int>(type: "integer", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    tenant_id = table.Column<int>(type: "INTEGER", nullable: false),
                    parent_id = table.Column<int>(type: "INTEGER", nullable: false),
                    child_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_module_post_association", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_module_post_association_mix_module_content_mix_module_content_id",
                        column: x => x.mix_module_content_id,
                        principalTable: "mix_module_content",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_mix_application_tenant_id",
                table: "mix_application",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_configuration_tenant_id",
                table: "mix_configuration",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_configuration_content_mix_configuration_id",
                table: "mix_configuration_content",
                column: "mix_configuration_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_configuration_content_mix_culture_id",
                table: "mix_configuration_content",
                column: "mix_culture_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_culture_tenant_id",
                table: "mix_culture",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_database_tenant_id",
                table: "mix_database",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_database_column_mix_database_id",
                table: "mix_database_column",
                column: "mix_database_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_database_context_tenant_id",
                table: "mix_database_context",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_database_relationship_child_id",
                table: "mix_database_relationship",
                column: "child_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_database_relationship_parent_id",
                table: "mix_database_relationship",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_domain_tenant_id",
                table: "mix_domain",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_language_tenant_id",
                table: "mix_language",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_language_content_mix_culture_id",
                table: "mix_language_content",
                column: "mix_culture_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_language_content_mix_language_id",
                table: "mix_language_content",
                column: "mix_language_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_media_tenant_id",
                table: "mix_media",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_tenant_id",
                table: "mix_module",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_content_mix_culture_id",
                table: "mix_module_content",
                column: "mix_culture_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_content_mix_module_id",
                table: "mix_module_content",
                column: "mix_module_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_mix_culture_id",
                table: "mix_module_data",
                column: "mix_culture_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_mix_module_content_id",
                table: "mix_module_data",
                column: "mix_module_content_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_post_association_mix_module_content_id",
                table: "mix_module_post_association",
                column: "mix_module_content_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_tenant_id",
                table: "mix_page",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_content_mix_culture_id",
                table: "mix_page_content",
                column: "mix_culture_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_content_mix_page_id",
                table: "mix_page_content",
                column: "mix_page_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_post_tenant_id",
                table: "mix_post",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_post_content_mix_culture_id",
                table: "mix_post_content",
                column: "mix_culture_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_post_content_mix_post_content_id",
                table: "mix_post_content",
                column: "mix_post_content_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_post_content_mix_post_id",
                table: "mix_post_content",
                column: "mix_post_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_template_mix_theme_id",
                table: "mix_template",
                column: "mix_theme_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_theme_tenant_id",
                table: "mix_theme",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_url_alias_tenant_id",
                table: "mix_url_alias",
                column: "tenant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mix_application");

            migrationBuilder.DropTable(
                name: "mix_configuration_content");

            migrationBuilder.DropTable(
                name: "mix_contributor");

            migrationBuilder.DropTable(
                name: "mix_database_association");

            migrationBuilder.DropTable(
                name: "mix_database_column");

            migrationBuilder.DropTable(
                name: "mix_database_context");

            migrationBuilder.DropTable(
                name: "mix_database_relationship");

            migrationBuilder.DropTable(
                name: "mix_discussion");

            migrationBuilder.DropTable(
                name: "mix_domain");

            migrationBuilder.DropTable(
                name: "mix_language_content");

            migrationBuilder.DropTable(
                name: "mix_media");

            migrationBuilder.DropTable(
                name: "mix_module_data");

            migrationBuilder.DropTable(
                name: "mix_module_post_association");

            migrationBuilder.DropTable(
                name: "mix_page_content");

            migrationBuilder.DropTable(
                name: "mix_page_module_association");

            migrationBuilder.DropTable(
                name: "mix_page_post_association");

            migrationBuilder.DropTable(
                name: "mix_post_content");

            migrationBuilder.DropTable(
                name: "mix_post_post_association");

            migrationBuilder.DropTable(
                name: "mix_template");

            migrationBuilder.DropTable(
                name: "mix_url_alias");

            migrationBuilder.DropTable(
                name: "mix_configuration");

            migrationBuilder.DropTable(
                name: "mix_database");

            migrationBuilder.DropTable(
                name: "mix_language");

            migrationBuilder.DropTable(
                name: "mix_module_content");

            migrationBuilder.DropTable(
                name: "mix_page");

            migrationBuilder.DropTable(
                name: "mix_post");

            migrationBuilder.DropTable(
                name: "mix_theme");

            migrationBuilder.DropTable(
                name: "mix_culture");

            migrationBuilder.DropTable(
                name: "mix_module");

            migrationBuilder.DropTable(
                name: "mix_tenant");
        }
    }
}

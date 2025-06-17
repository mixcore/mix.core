using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Cms.SqlServer
{
    /// <inheritdoc />
    public partial class UpdateTableRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mix_database_association");

            migrationBuilder.DropTable(
                name: "mix_database_column");

            migrationBuilder.DropTable(
                name: "mix_database_context");

            migrationBuilder.DropTable(
                name: "mix_database_relationship");

            migrationBuilder.DropTable(
                name: "mix_database");

            migrationBuilder.RenameColumn(
                name: "mix_db_id",
                table: "mix_theme",
                newName: "mix_db_table_id");

            migrationBuilder.RenameColumn(
                name: "mix_database_name",
                table: "mix_theme",
                newName: "mix_db_table_name");

            migrationBuilder.RenameColumn(
                name: "mix_db_id",
                table: "mix_post_content",
                newName: "mix_db_table_id");

            migrationBuilder.RenameColumn(
                name: "mix_database_name",
                table: "mix_post_content",
                newName: "mix_db_table_name");

            migrationBuilder.RenameColumn(
                name: "mix_db_id",
                table: "mix_page_content",
                newName: "mix_db_table_id");

            migrationBuilder.RenameColumn(
                name: "mix_database_name",
                table: "mix_page_content",
                newName: "mix_db_table_name");

            migrationBuilder.RenameColumn(
                name: "mix_db_id",
                table: "mix_module_content",
                newName: "mix_db_table_id");

            migrationBuilder.RenameColumn(
                name: "mix_database_name",
                table: "mix_module_content",
                newName: "mix_db_table_name");

            migrationBuilder.RenameColumn(
                name: "mix_db_id",
                table: "mix_application",
                newName: "mix_db_table_id");

            migrationBuilder.RenameColumn(
                name: "mix_database_name",
                table: "mix_application",
                newName: "mix_db_table_name");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "mix_media",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "newid()")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "mix_db_data_association",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    parent_database_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    child_database_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    guid_parent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    guid_child_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    parent_id = table.Column<int>(type: "int", nullable: false),
                    child_id = table.Column<int>(type: "int", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_db_data_association", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mix_db_database",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    database_provider = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    aes_key = table.Column<string>(type: "varchar(250)", nullable: false),
                    connection_string = table.Column<string>(type: "varchar(250)", nullable: false),
                    schema = table.Column<string>(type: "varchar(50)", nullable: false),
                    naming_convention = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    display_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    description = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    system_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_db_database", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_db_database_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_db_table",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mix_db_database_id = table.Column<int>(type: "int", nullable: true),
                    system_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    read_permissions = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    create_permissions = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    update_permissions = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    delete_permissions = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    self_managed = table.Column<bool>(type: "bit", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    display_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    description = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    tenant_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_db_table", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_db_table_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_db_column",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    system_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    display_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    mix_db_table_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    data_type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    configurations = table.Column<string>(type: "varchar(4000)", nullable: true),
                    reference_id = table.Column<int>(type: "int", nullable: true),
                    default_value = table.Column<string>(type: "ntext", nullable: true, collation: "Vietnamese_CI_AS"),
                    mix_db_table_id = table.Column<int>(type: "int", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_db_column", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_db_column_mix_db_table_mix_db_table_id",
                        column: x => x.mix_db_table_id,
                        principalTable: "mix_db_table",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mix_db_table_relationship",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parent_id = table.Column<int>(type: "int", nullable: false),
                    child_id = table.Column<int>(type: "int", nullable: false),
                    display_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    property_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    source_table_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    destinate_table_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    source_column_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    destinate_column_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_db_table_relationship", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_db_table_relationship_mix_db_table_child_id",
                        column: x => x.child_id,
                        principalTable: "mix_db_table",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_mix_db_table_relationship_mix_db_table_parent_id",
                        column: x => x.parent_id,
                        principalTable: "mix_db_table",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_mix_db_column_mix_db_table_id",
                table: "mix_db_column",
                column: "mix_db_table_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_db_database_tenant_id",
                table: "mix_db_database",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_db_table_tenant_id",
                table: "mix_db_table",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_db_table_relationship_child_id",
                table: "mix_db_table_relationship",
                column: "child_id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_db_table_relationship_parent_id",
                table: "mix_db_table_relationship",
                column: "parent_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mix_db_column");

            migrationBuilder.DropTable(
                name: "mix_db_data_association");

            migrationBuilder.DropTable(
                name: "mix_db_database");

            migrationBuilder.DropTable(
                name: "mix_db_table_relationship");

            migrationBuilder.DropTable(
                name: "mix_db_table");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_name",
                table: "mix_theme",
                newName: "mix_database_name");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_id",
                table: "mix_theme",
                newName: "mix_db_id");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_name",
                table: "mix_post_content",
                newName: "mix_database_name");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_id",
                table: "mix_post_content",
                newName: "mix_db_id");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_name",
                table: "mix_page_content",
                newName: "mix_database_name");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_id",
                table: "mix_page_content",
                newName: "mix_db_id");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_name",
                table: "mix_module_content",
                newName: "mix_database_name");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_id",
                table: "mix_module_content",
                newName: "mix_db_id");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_name",
                table: "mix_application",
                newName: "mix_database_name");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_id",
                table: "mix_application",
                newName: "mix_db_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "mix_media",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "mix_database",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    create_permissions = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    delete_permissions = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    display_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    mix_database_context_id = table.Column<int>(type: "int", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false),
                    read_permissions = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    self_managed = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    system_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    update_permissions = table.Column<string>(type: "nvarchar(250)", nullable: true)
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
                name: "mix_database_association",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    child_database_name = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    child_id = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    guid_child_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    guid_parent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    parent_database_name = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    parent_id = table.Column<int>(type: "int", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    tenant_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mix_database_association", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mix_database_context",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    aes_key = table.Column<string>(type: "varchar(250)", nullable: false),
                    connection_string = table.Column<string>(type: "varchar(250)", nullable: false),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    database_provider = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    display_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    naming_convention = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false),
                    schema = table.Column<string>(type: "varchar(50)", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    system_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS")
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
                name: "mix_database_column",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mix_database_id = table.Column<int>(type: "int", nullable: false),
                    configurations = table.Column<string>(type: "varchar(4000)", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    data_type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    default_value = table.Column<string>(type: "ntext", nullable: true, collation: "Vietnamese_CI_AS"),
                    display_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    mix_database_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false),
                    reference_id = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    system_name = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS")
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
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    child_id = table.Column<int>(type: "int", nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    destinate_database_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    display_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false),
                    source_database_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(50)", nullable: false)
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
        }
    }
}

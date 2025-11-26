using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

#nullable disable

namespace Mix.Database.Migrations.Cms.SqlServer
{
    /// <inheritdoc />
    public partial class UpdateTableRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "source_database_name",
                table: "mix_db_table_relationship",
                newName: "source_table_name");

            migrationBuilder.RenameColumn(
                name: "destinate_database_name",
                table: "mix_db_table_relationship",
                newName: "source_column_name");

            migrationBuilder.AddColumn<string>(
                name: "destinate_column_name",
                table: "mix_db_table_relationship",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "destinate_table_name",
                table: "mix_db_table_relationship",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "property_name",
                table: "mix_db_table_relationship",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.DropTable("mix_media");

            migrationBuilder.CreateTable(
                name: "mix_media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    extension = table.Column<string>(type: "nvarchar(50)", nullable: true, collation: "Vietnamese_CI_AS"),
                    file_folder = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    file_name = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    file_properties = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    file_type = table.Column<string>(type: "nvarchar(50)", nullable: true, collation: "Vietnamese_CI_AS"),
                    tags = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    source = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    target_url = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
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
                    table.PrimaryKey("pk_mix_media", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_media_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "destinate_column_name",
                table: "mix_db_table_relationship");

            migrationBuilder.DropColumn(
                name: "destinate_table_name",
                table: "mix_db_table_relationship");

            migrationBuilder.DropColumn(
                name: "property_name",
                table: "mix_db_table_relationship");

            migrationBuilder.RenameColumn(
                name: "source_table_name",
                table: "mix_db_table_relationship",
                newName: "source_database_name");

            migrationBuilder.RenameColumn(
                name: "source_column_name",
                table: "mix_db_table_relationship",
                newName: "destinate_database_name");

            migrationBuilder.CreateTable(
                name: "mix_media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    extension = table.Column<string>(type: "nvarchar(50)", nullable: true, collation: "Vietnamese_CI_AS"),
                    file_folder = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    file_name = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    file_properties = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    file_type = table.Column<string>(type: "nvarchar(50)", nullable: true, collation: "Vietnamese_CI_AS"),
                    tags = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    source = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    target_url = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
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
                    table.PrimaryKey("pk_mix_media", x => x.id);
                    table.ForeignKey(
                        name: "FK_mix_media_mix_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "mix_tenant",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

        }
    }
}

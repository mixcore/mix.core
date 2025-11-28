using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

#nullable disable

namespace Mix.Database.Migrations.Cms.MySql
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
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "destinate_table_name",
                table: "mix_db_table_relationship",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "property_name",
                table: "mix_db_table_relationship",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.DropTable("mix_media");

            migrationBuilder.CreateTable(
                name: "mix_media",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    extension = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    file_folder = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8"),
                    file_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    file_properties = table.Column<string>(type: "varchar(2000)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    file_type = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    tags = table.Column<string>(type: "varchar(2000)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    target_url = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    is_deleted = table.Column<sbyte>(type: "tinyint", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    description = table.Column<string>(type: "varchar(2000)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                    id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "(uuid())", collation: "ascii_general_ci"),
                    extension = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    file_folder = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8"),
                    file_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    file_properties = table.Column<string>(type: "varchar(2000)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    file_type = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    tags = table.Column<string>(type: "varchar(2000)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    target_url = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    is_deleted = table.Column<sbyte>(type: "tinyint", nullable: false),
                    display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    description = table.Column<string>(type: "varchar(2000)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

        }
    }
}

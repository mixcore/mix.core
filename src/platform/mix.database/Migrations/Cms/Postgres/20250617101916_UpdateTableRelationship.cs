using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mix.Database.Migrations.Cms.Postgres
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
                   id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                   extension = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                   file_folder = table.Column<string>(type: "varchar(250)", nullable: true),
                   file_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                   file_properties = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                   file_size = table.Column<long>(type: "bigint", nullable: false),
                   file_type = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                   tags = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                   source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                   target_url = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                   created_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                   last_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                   created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                   modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                   priority = table.Column<int>(type: "int", nullable: false),
                   status = table.Column<string>(type: "varchar(50)", nullable: false),
                   is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                   display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                   description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
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
                   id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                   extension = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                   file_folder = table.Column<string>(type: "varchar(250)", nullable: true),
                   file_name = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                   file_properties = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                   file_size = table.Column<long>(type: "bigint", nullable: false),
                   file_type = table.Column<string>(type: "varchar(50)", nullable: true, collation: "und-x-icu"),
                   tags = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
                   source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                   target_url = table.Column<string>(type: "varchar(250)", nullable: true, collation: "und-x-icu"),
                   created_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                   last_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                   created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                   modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                   priority = table.Column<int>(type: "int", nullable: false),
                   status = table.Column<string>(type: "varchar(50)", nullable: false),
                   is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                   display_name = table.Column<string>(type: "varchar(250)", nullable: false, collation: "und-x-icu"),
                   description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "und-x-icu"),
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

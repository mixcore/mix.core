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

            migrationBuilder.RenameColumn(
                name: "mix_db_id",
                table: "mix_application",
                newName: "mix_db_table_id");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "mix_media",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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

            migrationBuilder.RenameColumn(
                name: "mix_db_table_id",
                table: "mix_application",
                newName: "mix_db_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "mix_media",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}

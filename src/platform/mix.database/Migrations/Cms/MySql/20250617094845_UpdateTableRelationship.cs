using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

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

            migrationBuilder.RenameColumn(
                name: "mix_db_id",
                table: "mix_application",
                newName: "mix_db_table_id");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "mix_media",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "(uuid())")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

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
                type: "char(36)",
                nullable: false,
                defaultValueSql: "(uuid())",
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }
    }
}

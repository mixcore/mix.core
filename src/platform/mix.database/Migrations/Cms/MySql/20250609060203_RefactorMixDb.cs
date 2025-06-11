using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Cms.MySql
{
    /// <inheritdoc />
    /// <inheritdoc />
    public partial class RefactorMixDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
               name: "mix_database_association",
               newName: "mix_db_data_association");

            migrationBuilder.RenameTable(
                name: "mix_database_column",
                newName: "mix_db_column");

            migrationBuilder.RenameTable(
                name: "mix_database_context",
                newName: "mix_db_database");

            migrationBuilder.RenameTable(
                name: "mix_database_relationship",
                newName: "mix_db_table_relationship");

            migrationBuilder.RenameTable(
                name: "mix_database",
                newName: "mix_db_table");

            migrationBuilder.RenameColumn(
                name: "mix_database_name",
                table: "mix_application",
                newName: "mix_db_table_name");

            migrationBuilder.RenameColumn(
               name: "mix_db_id",
               table: "mix_application",
               newName: "mix_db_table_id");

            migrationBuilder.RenameColumn(
                name: "mix_database_name",
                table: "mix_db_column",
                newName: "mix_db_table_name");

            migrationBuilder.RenameColumn(
                name: "mix_database_context_id",
                table: "mix_db_table",
                newName: "mix_db_database_id");


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
                table: "mix_post_content",
                newName: "mix_db_table_id");


            migrationBuilder.RenameColumn(
                name: "mix_database_name",
                table: "mix_post_content",
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
                table: "mix_theme",
                newName: "mix_db_table_id");


            migrationBuilder.RenameColumn(
                name: "mix_database_name",
                table: "mix_theme",
                newName: "mix_db_table_name");

            migrationBuilder.RenameColumn(
                name: "mix_database_id",
                table: "mix_db_column",
                newName: "mix_db_table_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations.MySqlMixCms
{
    public partial class AddEditorValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EditorType",
                table: "mix_post",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "Html",
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "EditorValue",
                table: "mix_post",
                type: "text",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "EditorType",
                table: "mix_page",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "Html",
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "EditorValue",
                table: "mix_page",
                type: "text",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "EditorType",
                table: "mix_module",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "Html",
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "EditorValue",
                table: "mix_module",
                type: "text",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "EditorType",
                table: "mix_database_data_value",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "Html",
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "EditorValue",
                table: "mix_database_data_value",
                type: "text",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditorType",
                table: "mix_post");

            migrationBuilder.DropColumn(
                name: "EditorValue",
                table: "mix_post");

            migrationBuilder.DropColumn(
                name: "EditorType",
                table: "mix_page");

            migrationBuilder.DropColumn(
                name: "EditorValue",
                table: "mix_page");

            migrationBuilder.DropColumn(
                name: "EditorType",
                table: "mix_module");

            migrationBuilder.DropColumn(
                name: "EditorValue",
                table: "mix_module");

            migrationBuilder.DropColumn(
                name: "EditorType",
                table: "mix_database_data_value");

            migrationBuilder.DropColumn(
                name: "EditorValue",
                table: "mix_database_data_value");
        }
    }
}

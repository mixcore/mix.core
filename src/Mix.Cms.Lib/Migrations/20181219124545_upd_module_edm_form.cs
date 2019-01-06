using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class upd_module_edm_form : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "mix_page_article",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "FormTemplate",
                table: "mix_module",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EdmTemplate",
                table: "mix_module",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EdmTemplate",
                table: "mix_module");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "mix_page_article",
                nullable: true,
                defaultValueSql: "((0))",
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "FormTemplate",
                table: "mix_module",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);
        }
    }
}

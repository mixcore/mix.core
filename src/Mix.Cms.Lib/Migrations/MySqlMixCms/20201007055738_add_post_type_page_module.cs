using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations.MySqlMixCms
{
    public partial class add_post_type_page_module : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostType",
                table: "mix_page",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostType",
                table: "mix_module",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "DoubleValue",
                table: "mix_attribute_set_value",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostType",
                table: "mix_page");

            migrationBuilder.DropColumn(
                name: "PostType",
                table: "mix_module");

            migrationBuilder.AlterColumn<float>(
                name: "DoubleValue",
                table: "mix_attribute_set_value",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}

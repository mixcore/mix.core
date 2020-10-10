using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostType",
                table: "mix_page");

            migrationBuilder.DropColumn(
                name: "PostType",
                table: "mix_module");
        }
    }
}

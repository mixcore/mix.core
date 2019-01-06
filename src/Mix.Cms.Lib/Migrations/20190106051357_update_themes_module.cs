using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class update_themes_module : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "mix_theme",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "mix_theme",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "mix_module",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "mix_theme");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "mix_theme");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "mix_module");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class upd_media_targeturl_source : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "mix_media",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetUrl",
                table: "mix_media",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "mix_media");

            migrationBuilder.DropColumn(
                name: "TargetUrl",
                table: "mix_media");
        }
    }
}

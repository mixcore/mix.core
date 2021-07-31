using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class AddExcerptCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Excerpt",
                table: "MixPostContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "Excerpt",
                table: "MixPageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "Excerpt",
                table: "MixModuleContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "Excerpt",
                table: "MixDataContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Excerpt",
                table: "MixPostContent");

            migrationBuilder.DropColumn(
                name: "Excerpt",
                table: "MixPageContent");

            migrationBuilder.DropColumn(
                name: "Excerpt",
                table: "MixModuleContent");

            migrationBuilder.DropColumn(
                name: "Excerpt",
                table: "MixDataContent");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class u1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileContent",
                table: "MixViewTemplate");

            migrationBuilder.DropColumn(
                name: "SpaContent",
                table: "MixViewTemplate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobileContent",
                table: "MixViewTemplate",
                type: "ntext",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "SpaContent",
                table: "MixViewTemplate",
                type: "ntext",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE");
        }
    }
}

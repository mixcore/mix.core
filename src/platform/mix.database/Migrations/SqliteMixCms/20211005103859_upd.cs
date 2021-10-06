using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class upd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SystemName",
                table: "MixTheme",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemName",
                table: "MixTheme");
        }
    }
}

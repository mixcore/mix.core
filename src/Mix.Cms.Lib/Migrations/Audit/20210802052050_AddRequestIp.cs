using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations.Audit
{
    public partial class AddRequestIp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestIp",
                table: "AuditLog",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestIp",
                table: "AuditLog");
        }
    }
}

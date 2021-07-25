using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations.Audit
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Endpoint = table.Column<string>(type: "TEXT", nullable: true),
                    Method = table.Column<string>(type: "TEXT", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: true),
                    Success = table.Column<bool>(type: "INTEGER", nullable: false),
                    Exception = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.AuditLogDb
{
    /// <inheritdoc />
    public partial class AddRespQueryCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QueryString",
                table: "AuditLog",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Response",
                table: "AuditLog",
                type: "ntext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QueryString",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "Response",
                table: "AuditLog");
        }
    }
}

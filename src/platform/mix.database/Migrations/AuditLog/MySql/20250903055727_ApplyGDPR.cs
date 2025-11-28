using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.AuditLog.MySql
{
    /// <inheritdoc />
    public partial class ApplyGDPR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "correlation_id",
                table: "mix_audit_log",
                type: "varchar(250)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<sbyte>(
                name: "phi_access_flag",
                table: "mix_audit_log",
                type: "tinyint",
                nullable: false,
                defaultValue: (sbyte)0);

            migrationBuilder.AddColumn<string>(
                name: "session_id",
                table: "mix_audit_log",
                type: "varchar(250)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "tenant_id",
                table: "mix_audit_log",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "user_agent",
                table: "mix_audit_log",
                type: "varchar(2000)",
                maxLength: 500,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_CorrelationId",
                table: "mix_audit_log",
                column: "correlation_id");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Tenant_PHI",
                table: "mix_audit_log",
                columns: new[] { "tenant_id", "phi_access_flag" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditLog_CorrelationId",
                table: "mix_audit_log");

            migrationBuilder.DropIndex(
                name: "IX_AuditLog_Tenant_PHI",
                table: "mix_audit_log");

            migrationBuilder.DropColumn(
                name: "correlation_id",
                table: "mix_audit_log");

            migrationBuilder.DropColumn(
                name: "phi_access_flag",
                table: "mix_audit_log");

            migrationBuilder.DropColumn(
                name: "session_id",
                table: "mix_audit_log");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "mix_audit_log");

            migrationBuilder.DropColumn(
                name: "user_agent",
                table: "mix_audit_log");
        }
    }
}

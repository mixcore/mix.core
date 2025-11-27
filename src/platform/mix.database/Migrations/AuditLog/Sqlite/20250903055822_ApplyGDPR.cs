using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.AuditLog.Sqlite
{
    /// <inheritdoc />
    public partial class ApplyGDPR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "success",
                table: "mix_audit_log",
                type: "integer",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "mix_audit_log",
                type: "integer",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "mix_audit_log",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "hex(randomblob(16))",
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AddColumn<string>(
                name: "correlation_id",
                table: "mix_audit_log",
                type: "varchar(250)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "phi_access_flag",
                table: "mix_audit_log",
                type: "integer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "session_id",
                table: "mix_audit_log",
                type: "varchar(250)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tenant_id",
                table: "mix_audit_log",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "user_agent",
                table: "mix_audit_log",
                type: "varchar(4000)",
                maxLength: 500,
                nullable: true);

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

            migrationBuilder.AlterColumn<bool>(
                name: "success",
                table: "mix_audit_log",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "mix_audit_log",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "mix_audit_log",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldDefaultValueSql: "hex(randomblob(16))");
        }
    }
}

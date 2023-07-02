using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.AuditLogDb
{
    /// <inheritdoc />
    public partial class AddStatusCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AuditLog",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldDefaultValueSql: "(newid())");

            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "AuditLog",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "AuditLog");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AuditLog",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "(newid())",
                oldClrType: typeof(Guid),
                oldType: "TEXT");
        }
    }
}

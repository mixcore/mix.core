using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.AuditLog.SqlServer
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mix_audit_log",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    success = table.Column<bool>(type: "bit", nullable: false),
                    status_code = table.Column<int>(type: "int", nullable: false),
                    response_time = table.Column<int>(type: "int", nullable: false),
                    request_ip = table.Column<string>(type: "varchar(50)", nullable: true),
                    endpoint = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    method = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    query_string = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    body = table.Column<string>(type: "ntext", nullable: true),
                    response = table.Column<string>(type: "ntext", nullable: true),
                    exception = table.Column<string>(type: "ntext", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_log", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mix_audit_log");
        }
    }
}

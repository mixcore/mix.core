using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.AuditLog.Postgres
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audit_log",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    success = table.Column<bool>(type: "boolean", nullable: false),
                    status_code = table.Column<int>(type: "int", nullable: false),
                    response_time = table.Column<int>(type: "int", nullable: false),
                    request_ip = table.Column<string>(type: "varchar(50)", nullable: true),
                    endpoint = table.Column<string>(type: "varchar(4000)", nullable: true),
                    method = table.Column<string>(type: "varchar(50)", nullable: true),
                    query_string = table.Column<string>(type: "varchar(4000)", nullable: true),
                    body = table.Column<string>(type: "text", nullable: true),
                    response = table.Column<string>(type: "text", nullable: true),
                    exception = table.Column<string>(type: "text", nullable: true),
                    created_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    modified_by = table.Column<string>(type: "varchar(250)", nullable: true),
                    priority = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "audit_log");
        }
    }
}

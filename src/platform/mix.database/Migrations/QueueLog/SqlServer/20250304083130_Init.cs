using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.QueueLog.SqlServer
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mix_queue_log",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    queue_message_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    topic_id = table.Column<string>(type: "varchar(250)", nullable: true),
                    subscription_id = table.Column<string>(type: "varchar(250)", nullable: true),
                    action = table.Column<string>(type: "varchar(250)", nullable: true),
                    string_data = table.Column<string>(type: "ntext", nullable: true),
                    object_data = table.Column<string>(type: "ntext", nullable: true),
                    exception = table.Column<string>(type: "ntext", nullable: true),
                    subscriptions = table.Column<string>(type: "ntext", nullable: true),
                    data_type_full_name = table.Column<string>(type: "varchar(250)", nullable: true),
                    note = table.Column<string>(type: "varchar(250)", nullable: true),
                    state = table.Column<string>(type: "varchar(50)", nullable: false),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("pk_queue_log", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mix_queue_log");
        }
    }
}

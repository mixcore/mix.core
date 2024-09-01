using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqlITEQueueLogDb
{
    /// <inheritdoc />
    public partial class InitQueueLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QueueLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    QueueMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    TopicId = table.Column<string>(type: "varchar(250)", nullable: true),
                    SubscriptionId = table.Column<string>(type: "varchar(250)", nullable: true),
                    Action = table.Column<string>(type: "varchar(250)", nullable: true),
                    StringData = table.Column<string>(type: "text", nullable: true),
                    ObjectData = table.Column<string>(type: "text", nullable: true),
                    Exception = table.Column<string>(type: "text", nullable: true),
                    Subscriptions = table.Column<string>(type: "text", nullable: true),
                    DataTypeFullName = table.Column<string>(type: "varchar(250)", nullable: true),
                    Note = table.Column<string>(type: "varchar(250)", nullable: true),
                    State = table.Column<string>(type: "varchar(50)", nullable: false),
                    TenantId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueLog", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueueLog");
        }
    }
}

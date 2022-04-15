using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SQLiteQuartzDb
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QRTZ_CALENDARS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "NVARCHAR(120)", nullable: false),
                    CALENDAR_NAME = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    CALENDAR = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_CALENDARS", x => new { x.SCHED_NAME, x.CALENDAR_NAME });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_FIRED_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "NVARCHAR(120)", nullable: false),
                    ENTRY_ID = table.Column<string>(type: "NVARCHAR(140)", nullable: false),
                    TRIGGER_NAME = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    INSTANCE_NAME = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    FIRED_TIME = table.Column<long>(type: "BIGINT", nullable: false),
                    SCHED_TIME = table.Column<long>(type: "BIGINT", nullable: false),
                    PRIORITY = table.Column<int>(type: "INTEGER", nullable: false),
                    STATE = table.Column<string>(type: "NVARCHAR(16)", nullable: false),
                    JOB_NAME = table.Column<string>(type: "NVARCHAR(150)", nullable: true),
                    JOB_GROUP = table.Column<string>(type: "NVARCHAR(150)", nullable: true),
                    IS_NONCONCURRENT = table.Column<bool>(type: "BIT", nullable: true),
                    REQUESTS_RECOVERY = table.Column<bool>(type: "BIT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_FIRED_TRIGGERS", x => new { x.SCHED_NAME, x.ENTRY_ID });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_JOB_DETAILS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "NVARCHAR(120)", nullable: false),
                    JOB_NAME = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    JOB_GROUP = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    JOB_CLASS_NAME = table.Column<string>(type: "NVARCHAR(250)", nullable: false),
                    IS_DURABLE = table.Column<bool>(type: "BIT", nullable: false),
                    IS_NONCONCURRENT = table.Column<bool>(type: "BIT", nullable: false),
                    IS_UPDATE_DATA = table.Column<bool>(type: "BIT", nullable: false),
                    REQUESTS_RECOVERY = table.Column<bool>(type: "BIT", nullable: false),
                    JOB_DATA = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_JOB_DETAILS", x => new { x.SCHED_NAME, x.JOB_NAME, x.JOB_GROUP });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_LOCKS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "NVARCHAR(120)", nullable: false),
                    LOCK_NAME = table.Column<string>(type: "NVARCHAR(40)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_LOCKS", x => new { x.SCHED_NAME, x.LOCK_NAME });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_PAUSED_TRIGGER_GRPS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "NVARCHAR(120)", nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "NVARCHAR(150)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_PAUSED_TRIGGER_GRPS", x => new { x.SCHED_NAME, x.TRIGGER_GROUP });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_SCHEDULER_STATE",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "NVARCHAR(120)", nullable: false),
                    INSTANCE_NAME = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    LAST_CHECKIN_TIME = table.Column<long>(type: "BIGINT", nullable: false),
                    CHECKIN_INTERVAL = table.Column<long>(type: "BIGINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_SCHEDULER_STATE", x => new { x.SCHED_NAME, x.INSTANCE_NAME });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "NVARCHAR(120)", nullable: false),
                    TRIGGER_NAME = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    JOB_NAME = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    JOB_GROUP = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    NEXT_FIRE_TIME = table.Column<long>(type: "BIGINT", nullable: true),
                    PREV_FIRE_TIME = table.Column<long>(type: "BIGINT", nullable: true),
                    PRIORITY = table.Column<int>(type: "INTEGER", nullable: true),
                    TRIGGER_STATE = table.Column<string>(type: "NVARCHAR(16)", nullable: false),
                    TRIGGER_TYPE = table.Column<string>(type: "NVARCHAR(8)", nullable: false),
                    START_TIME = table.Column<long>(type: "BIGINT", nullable: false),
                    END_TIME = table.Column<long>(type: "BIGINT", nullable: true),
                    CALENDAR_NAME = table.Column<string>(type: "NVARCHAR(200)", nullable: true),
                    MISFIRE_INSTR = table.Column<short>(type: "INTEGER", nullable: true),
                    JOB_DATA = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_TRIGGERS_QRTZ_JOB_DETAILS_SCHED_NAME_JOB_NAME_JOB_GROUP",
                        columns: x => new { x.SCHED_NAME, x.JOB_NAME, x.JOB_GROUP },
                        principalTable: "QRTZ_JOB_DETAILS",
                        principalColumns: new[] { "SCHED_NAME", "JOB_NAME", "JOB_GROUP" });
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_BLOB_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "NVARCHAR(120)", nullable: false),
                    TRIGGER_NAME = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    BLOB_DATA = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_BLOB_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_BLOB_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_TRIGGER_GROUP",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "QRTZ_TRIGGERS",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_CRON_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "NVARCHAR(120)", nullable: false),
                    TRIGGER_NAME = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    CRON_EXPRESSION = table.Column<string>(type: "NVARCHAR(250)", nullable: false),
                    TIME_ZONE_ID = table.Column<string>(type: "NVARCHAR(80)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_CRON_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_CRON_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_TRIGGER_GROUP",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "QRTZ_TRIGGERS",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_SIMPLE_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "NVARCHAR(120)", nullable: false),
                    TRIGGER_NAME = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "NVARCHAR(150)", nullable: false),
                    REPEAT_COUNT = table.Column<long>(type: "BIGINT", nullable: false),
                    REPEAT_INTERVAL = table.Column<long>(type: "BIGINT", nullable: false),
                    TIMES_TRIGGERED = table.Column<long>(type: "BIGINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_SIMPLE_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_SIMPLE_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_TRIGGER_GROUP",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "QRTZ_TRIGGERS",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QRTZ_SIMPROP_TRIGGERS",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "NVARCHAR (120)", nullable: false),
                    TRIGGER_NAME = table.Column<string>(type: "NVARCHAR (150)", nullable: false),
                    TRIGGER_GROUP = table.Column<string>(type: "NVARCHAR (150)", nullable: false),
                    STR_PROP_1 = table.Column<string>(type: "NVARCHAR (512)", nullable: true),
                    STR_PROP_2 = table.Column<string>(type: "NVARCHAR (512)", nullable: true),
                    STR_PROP_3 = table.Column<string>(type: "NVARCHAR (512)", nullable: true),
                    INT_PROP_1 = table.Column<int>(type: "INT", nullable: true),
                    INT_PROP_2 = table.Column<int>(type: "INT", nullable: true),
                    LONG_PROP_1 = table.Column<long>(type: "BIGINT", nullable: true),
                    LONG_PROP_2 = table.Column<long>(type: "BIGINT", nullable: true),
                    DEC_PROP_1 = table.Column<decimal>(type: "NUMERIC", nullable: true),
                    DEC_PROP_2 = table.Column<decimal>(type: "NUMERIC", nullable: true),
                    BOOL_PROP_1 = table.Column<bool>(type: "BIT", nullable: true),
                    BOOL_PROP_2 = table.Column<bool>(type: "BIT", nullable: true),
                    TIME_ZONE_ID = table.Column<string>(type: "NVARCHAR(80)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRTZ_SIMPROP_TRIGGERS", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP });
                    table.ForeignKey(
                        name: "FK_QRTZ_SIMPROP_TRIGGERS_QRTZ_TRIGGERS_SCHED_NAME_TRIGGER_NAME_TRIGGER_GROUP",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "QRTZ_TRIGGERS",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QRTZ_TRIGGERS_SCHED_NAME_JOB_NAME_JOB_GROUP",
                table: "QRTZ_TRIGGERS",
                columns: new[] { "SCHED_NAME", "JOB_NAME", "JOB_GROUP" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QRTZ_BLOB_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_CALENDARS");

            migrationBuilder.DropTable(
                name: "QRTZ_CRON_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_FIRED_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_LOCKS");

            migrationBuilder.DropTable(
                name: "QRTZ_PAUSED_TRIGGER_GRPS");

            migrationBuilder.DropTable(
                name: "QRTZ_SCHEDULER_STATE");

            migrationBuilder.DropTable(
                name: "QRTZ_SIMPLE_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_SIMPROP_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_TRIGGERS");

            migrationBuilder.DropTable(
                name: "QRTZ_JOB_DETAILS");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.MySQLQuartzDb
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "qrtz_calendars",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CALENDAR_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CALENDAR = table.Column<byte[]>(type: "blob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.SCHED_NAME, x.CALENDAR_NAME })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "qrtz_fired_triggers",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ENTRY_ID = table.Column<string>(type: "varchar(140)", maxLength: 140, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    INSTANCE_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FIRED_TIME = table.Column<long>(type: "bigint(19)", nullable: false),
                    SCHED_TIME = table.Column<long>(type: "bigint(19)", nullable: false),
                    PRIORITY = table.Column<int>(type: "int(11)", nullable: false),
                    STATE = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_GROUP = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IS_NONCONCURRENT = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    REQUESTS_RECOVERY = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.SCHED_NAME, x.ENTRY_ID })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "qrtz_job_details",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_GROUP = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESCRIPTION = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_CLASS_NAME = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IS_DURABLE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IS_NONCONCURRENT = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IS_UPDATE_DATA = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    REQUESTS_RECOVERY = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    JOB_DATA = table.Column<byte[]>(type: "blob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.SCHED_NAME, x.JOB_NAME, x.JOB_GROUP })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "qrtz_locks",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LOCK_NAME = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.SCHED_NAME, x.LOCK_NAME })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "qrtz_paused_trigger_grps",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.SCHED_NAME, x.TRIGGER_GROUP })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "qrtz_scheduler_state",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    INSTANCE_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LAST_CHECKIN_TIME = table.Column<long>(type: "bigint(19)", nullable: false),
                    CHECKIN_INTERVAL = table.Column<long>(type: "bigint(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.SCHED_NAME, x.INSTANCE_NAME })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "qrtz_triggers",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JOB_GROUP = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESCRIPTION = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NEXT_FIRE_TIME = table.Column<long>(type: "bigint(19)", nullable: true),
                    PREV_FIRE_TIME = table.Column<long>(type: "bigint(19)", nullable: true),
                    PRIORITY = table.Column<int>(type: "int(11)", nullable: true),
                    TRIGGER_STATE = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_TYPE = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    START_TIME = table.Column<long>(type: "bigint(19)", nullable: false),
                    END_TIME = table.Column<long>(type: "bigint(19)", nullable: true),
                    CALENDAR_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MISFIRE_INSTR = table.Column<short>(type: "smallint(2)", nullable: true),
                    JOB_DATA = table.Column<byte[]>(type: "blob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });
                    table.ForeignKey(
                        name: "qrtz_triggers_ibfk_1",
                        columns: x => new { x.SCHED_NAME, x.JOB_NAME, x.JOB_GROUP },
                        principalTable: "qrtz_job_details",
                        principalColumns: new[] { "SCHED_NAME", "JOB_NAME", "JOB_GROUP" });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "qrtz_blob_triggers",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BLOB_DATA = table.Column<byte[]>(type: "blob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });
                    table.ForeignKey(
                        name: "qrtz_blob_triggers_ibfk_1",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "qrtz_triggers",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "qrtz_cron_triggers",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CRON_EXPRESSION = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TIME_ZONE_ID = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });
                    table.ForeignKey(
                        name: "qrtz_cron_triggers_ibfk_1",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "qrtz_triggers",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "qrtz_simple_triggers",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    REPEAT_COUNT = table.Column<long>(type: "bigint(7)", nullable: false),
                    REPEAT_INTERVAL = table.Column<long>(type: "bigint(12)", nullable: false),
                    TIMES_TRIGGERED = table.Column<long>(type: "bigint(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });
                    table.ForeignKey(
                        name: "qrtz_simple_triggers_ibfk_1",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "qrtz_triggers",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "qrtz_simprop_triggers",
                columns: table => new
                {
                    SCHED_NAME = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_NAME = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRIGGER_GROUP = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    STR_PROP_1 = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    STR_PROP_2 = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    STR_PROP_3 = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    INT_PROP_1 = table.Column<int>(type: "int(11)", nullable: true),
                    INT_PROP_2 = table.Column<int>(type: "int(11)", nullable: true),
                    LONG_PROP_1 = table.Column<long>(type: "bigint(20)", nullable: true),
                    LONG_PROP_2 = table.Column<long>(type: "bigint(20)", nullable: true),
                    DEC_PROP_1 = table.Column<decimal>(type: "decimal(13,4)", precision: 13, scale: 4, nullable: true),
                    DEC_PROP_2 = table.Column<decimal>(type: "decimal(13,4)", precision: 13, scale: 4, nullable: true),
                    BOOL_PROP_1 = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    BOOL_PROP_2 = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    TIME_ZONE_ID = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });
                    table.ForeignKey(
                        name: "qrtz_simprop_triggers_ibfk_1",
                        columns: x => new { x.SCHED_NAME, x.TRIGGER_NAME, x.TRIGGER_GROUP },
                        principalTable: "qrtz_triggers",
                        principalColumns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" });
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "SCHED_NAME",
                table: "qrtz_blob_triggers",
                columns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_FT_INST_JOB_REQ_RCVRY",
                table: "qrtz_fired_triggers",
                columns: new[] { "SCHED_NAME", "INSTANCE_NAME", "REQUESTS_RECOVERY" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_FT_J_G",
                table: "qrtz_fired_triggers",
                columns: new[] { "SCHED_NAME", "JOB_NAME", "JOB_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_FT_JG",
                table: "qrtz_fired_triggers",
                columns: new[] { "SCHED_NAME", "JOB_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_FT_T_G",
                table: "qrtz_fired_triggers",
                columns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_FT_TG",
                table: "qrtz_fired_triggers",
                columns: new[] { "SCHED_NAME", "TRIGGER_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_FT_TRIG_INST_NAME",
                table: "qrtz_fired_triggers",
                columns: new[] { "SCHED_NAME", "INSTANCE_NAME" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_J_GRP",
                table: "qrtz_job_details",
                columns: new[] { "SCHED_NAME", "JOB_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_J_REQ_RECOVERY",
                table: "qrtz_job_details",
                columns: new[] { "SCHED_NAME", "REQUESTS_RECOVERY" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_C",
                table: "qrtz_triggers",
                columns: new[] { "SCHED_NAME", "CALENDAR_NAME" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_G",
                table: "qrtz_triggers",
                columns: new[] { "SCHED_NAME", "TRIGGER_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_J",
                table: "qrtz_triggers",
                columns: new[] { "SCHED_NAME", "JOB_NAME", "JOB_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_JG",
                table: "qrtz_triggers",
                columns: new[] { "SCHED_NAME", "JOB_GROUP" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_N_G_STATE",
                table: "qrtz_triggers",
                columns: new[] { "SCHED_NAME", "TRIGGER_GROUP", "TRIGGER_STATE" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_N_STATE",
                table: "qrtz_triggers",
                columns: new[] { "SCHED_NAME", "TRIGGER_NAME", "TRIGGER_GROUP", "TRIGGER_STATE" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_NEXT_FIRE_TIME",
                table: "qrtz_triggers",
                columns: new[] { "SCHED_NAME", "NEXT_FIRE_TIME" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_NFT_MISFIRE",
                table: "qrtz_triggers",
                columns: new[] { "SCHED_NAME", "MISFIRE_INSTR", "NEXT_FIRE_TIME" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_NFT_ST",
                table: "qrtz_triggers",
                columns: new[] { "SCHED_NAME", "TRIGGER_STATE", "NEXT_FIRE_TIME" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_NFT_ST_MISFIRE",
                table: "qrtz_triggers",
                columns: new[] { "SCHED_NAME", "MISFIRE_INSTR", "NEXT_FIRE_TIME", "TRIGGER_STATE" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_NFT_ST_MISFIRE_GRP",
                table: "qrtz_triggers",
                columns: new[] { "SCHED_NAME", "MISFIRE_INSTR", "NEXT_FIRE_TIME", "TRIGGER_GROUP", "TRIGGER_STATE" });

            migrationBuilder.CreateIndex(
                name: "IDX_QRTZ_T_STATE",
                table: "qrtz_triggers",
                columns: new[] { "SCHED_NAME", "TRIGGER_STATE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "qrtz_blob_triggers");

            migrationBuilder.DropTable(
                name: "qrtz_calendars");

            migrationBuilder.DropTable(
                name: "qrtz_cron_triggers");

            migrationBuilder.DropTable(
                name: "qrtz_fired_triggers");

            migrationBuilder.DropTable(
                name: "qrtz_locks");

            migrationBuilder.DropTable(
                name: "qrtz_paused_trigger_grps");

            migrationBuilder.DropTable(
                name: "qrtz_scheduler_state");

            migrationBuilder.DropTable(
                name: "qrtz_simple_triggers");

            migrationBuilder.DropTable(
                name: "qrtz_simprop_triggers");

            migrationBuilder.DropTable(
                name: "qrtz_triggers");

            migrationBuilder.DropTable(
                name: "qrtz_job_details");
        }
    }
}

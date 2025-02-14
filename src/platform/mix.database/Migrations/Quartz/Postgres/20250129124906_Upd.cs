using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Quartz.Postgres
{
    /// <inheritdoc />
    public partial class Upd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "qrtz_blob_triggers_sched_name_trigger_name_trigger_group_fkey",
                table: "qrtz_blob_triggers");

            migrationBuilder.AlterColumn<int>(
                name: "misfire_instr",
                table: "qrtz_triggers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "times_triggered",
                table: "qrtz_simple_triggers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "repeat_count",
                table: "qrtz_simple_triggers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "misfire_instr",
                table: "qrtz_triggers",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "times_triggered",
                table: "qrtz_simple_triggers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "repeat_count",
                table: "qrtz_simple_triggers",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "qrtz_blob_triggers_sched_name_trigger_name_trigger_group_fkey",
                table: "qrtz_blob_triggers",
                columns: new[] { "sched_name", "trigger_name", "trigger_group" },
                principalTable: "qrtz_triggers",
                principalColumns: new[] { "sched_name", "trigger_name", "trigger_group" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}

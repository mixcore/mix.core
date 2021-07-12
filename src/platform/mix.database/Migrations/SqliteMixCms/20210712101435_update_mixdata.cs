using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class update_mixdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataId",
                table: "MixDataContentValue");

            migrationBuilder.AlterColumn<Guid>(
                name: "MixDataId",
                table: "MixDataContent",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixData",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValueSql: "NEWID()")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataId",
                table: "MixDataContentValue",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MixDataId",
                table: "MixDataContent",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "MixData",
                type: "INTEGER",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldDefaultValueSql: "NEWID()")
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}

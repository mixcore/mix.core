using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class RemoveMixContentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDataContentValue_MixDataContent_MixDataContentId",
                table: "MixDataContentValue");

            migrationBuilder.AlterColumn<Guid>(
                name: "MixDataContentId",
                table: "MixDataContentValue",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDataContentValue_MixDataContent_MixDataContentId",
                table: "MixDataContentValue",
                column: "MixDataContentId",
                principalTable: "MixDataContent",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDataContentValue_MixDataContent_MixDataContentId",
                table: "MixDataContentValue");

            migrationBuilder.AlterColumn<Guid>(
                name: "MixDataContentId",
                table: "MixDataContentValue",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MixDataContentValue_MixDataContent_MixDataContentId",
                table: "MixDataContentValue",
                column: "MixDataContentId",
                principalTable: "MixDataContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

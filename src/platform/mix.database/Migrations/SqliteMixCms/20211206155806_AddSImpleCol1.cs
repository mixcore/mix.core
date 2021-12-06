using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class AddSImpleCol1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixModuleData_MixModule_MixModuleId",
                table: "MixModuleData");

            migrationBuilder.RenameColumn(
                name: "MixModuleId",
                table: "MixModuleData",
                newName: "MixModuleContentId");

            migrationBuilder.RenameColumn(
                name: "Fields",
                table: "MixModuleData",
                newName: "SimpleDataColumns");

            migrationBuilder.RenameIndex(
                name: "IX_MixModuleData_MixModuleId",
                table: "MixModuleData",
                newName: "IX_MixModuleData_MixModuleContentId");

            migrationBuilder.AddColumn<int>(
                name: "ModuleContentId",
                table: "MixModuleData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MixModuleData_MixModuleContent_MixModuleContentId",
                table: "MixModuleData",
                column: "MixModuleContentId",
                principalTable: "MixModuleContent",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixModuleData_MixModuleContent_MixModuleContentId",
                table: "MixModuleData");

            migrationBuilder.DropColumn(
                name: "ModuleContentId",
                table: "MixModuleData");

            migrationBuilder.RenameColumn(
                name: "SimpleDataColumns",
                table: "MixModuleData",
                newName: "Fields");

            migrationBuilder.RenameColumn(
                name: "MixModuleContentId",
                table: "MixModuleData",
                newName: "MixModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_MixModuleData_MixModuleContentId",
                table: "MixModuleData",
                newName: "IX_MixModuleData_MixModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_MixModuleData_MixModule_MixModuleId",
                table: "MixModuleData",
                column: "MixModuleId",
                principalTable: "MixModule",
                principalColumn: "Id");
        }
    }
}

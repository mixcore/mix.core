using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqlServerMixCms
{
    public partial class RemoveTenantIdFromMixDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabase_MixTenant_MixTenantId",
                table: "MixDatabase");

            migrationBuilder.DropColumn(
                name: "MixTenantId",
                table: "MixDatabaseRelationship");

            migrationBuilder.DropColumn(
                name: "MixTenantId",
                table: "MixDatabaseColumn");

            migrationBuilder.AlterColumn<int>(
                name: "MixTenantId",
                table: "MixDatabase",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabase_MixTenant_MixTenantId",
                table: "MixDatabase",
                column: "MixTenantId",
                principalTable: "MixTenant",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabase_MixTenant_MixTenantId",
                table: "MixDatabase");

            migrationBuilder.AddColumn<int>(
                name: "MixTenantId",
                table: "MixDatabaseRelationship",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MixTenantId",
                table: "MixDatabaseColumn",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MixTenantId",
                table: "MixDatabase",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabase_MixTenant_MixTenantId",
                table: "MixDatabase",
                column: "MixTenantId",
                principalTable: "MixTenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

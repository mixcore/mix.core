using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.PostgresqlMixCms
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
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MixTenantId",
                table: "MixDatabaseColumn",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MixTenantId",
                table: "MixDatabase",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
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

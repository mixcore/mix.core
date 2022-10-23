using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class UpdateDbPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WritePermissions",
                table: "MixDatabase",
                newName: "UpdatePermissions");

            migrationBuilder.AddColumn<string>(
                name: "CreatePermissions",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletePermissions",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SelfManaged",
                table: "MixDatabase",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatePermissions",
                table: "MixDatabase");

            migrationBuilder.DropColumn(
                name: "DeletePermissions",
                table: "MixDatabase");

            migrationBuilder.DropColumn(
                name: "SelfManaged",
                table: "MixDatabase");

            migrationBuilder.RenameColumn(
                name: "UpdatePermissions",
                table: "MixDatabase",
                newName: "WritePermissions");
        }
    }
}

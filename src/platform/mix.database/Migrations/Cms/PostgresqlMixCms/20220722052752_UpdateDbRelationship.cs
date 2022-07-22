using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.PostgresqlMixCms
{
    public partial class UpdateDbRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DestinateDatabaseName",
                table: "MixDatabaseRelationship",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceDatabaseName",
                table: "MixDatabaseRelationship",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinateDatabaseName",
                table: "MixDatabaseRelationship");

            migrationBuilder.DropColumn(
                name: "SourceDatabaseName",
                table: "MixDatabaseRelationship");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class AddContentIsPublic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixPostContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixPageContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixModuleData",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixModuleContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixLanguageContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixDataContentValue",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixDataContentAssociation",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixDataContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixConfigurationContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "MixPostContent");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "MixPageContent");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "MixModuleData");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "MixModuleContent");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "MixLanguageContent");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "MixDataContentValue");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "MixDataContentAssociation");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "MixDataContent");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "MixConfigurationContent");
        }
    }
}

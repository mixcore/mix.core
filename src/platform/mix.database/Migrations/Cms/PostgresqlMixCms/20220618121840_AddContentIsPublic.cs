using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.PostgresqlMixCms
{
    public partial class AddContentIsPublic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixPostContent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixPageContent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixModuleData",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixModuleContent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixLanguageContent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixDataContentValue",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixDataContentAssociation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixDataContent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixConfigurationContent",
                type: "boolean",
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

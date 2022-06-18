using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqlServerMixCms
{
    public partial class AddContentIsPublic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixPostContent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixPageContent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixModuleData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixModuleContent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixLanguageContent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixDataContentValue",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixDataContentAssociation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixDataContent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MixConfigurationContent",
                type: "bit",
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

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqlServerMixCms
{
    public partial class AddContentIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "MixContributor");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixPostContent",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixPageContent",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixModuleContent",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixLanguageContent",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixDataContentValue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixDataContentAssociation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixDataContent",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "MixContributor",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "MixContributor",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixConfigurationContent",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MixPostContent");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MixPageContent");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MixModuleData");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MixModuleContent");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MixLanguageContent");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MixDataContentValue");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MixDataContentAssociation");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MixDataContent");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "MixContributor");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MixContributor");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MixConfigurationContent");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "MixContributor",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "",
                collation: "Vietnamese_CI_AS");
        }
    }
}

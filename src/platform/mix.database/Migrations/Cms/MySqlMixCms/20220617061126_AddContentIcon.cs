using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations
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
                type: "varchar(50)",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixPageContent",
                type: "varchar(50)",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixModuleData",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixModuleContent",
                type: "varchar(50)",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixLanguageContent",
                type: "varchar(50)",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixDataContentValue",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixDataContentAssociation",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixDataContent",
                type: "varchar(50)",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "MixContributor",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "MixContributor",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixConfigurationContent",
                type: "varchar(50)",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");
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
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");
        }
    }
}

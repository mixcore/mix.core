using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqlServerMixCms
{
    public partial class UpdateConfigLanguageContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixLanguageContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "MixLanguageContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<string>(
                name: "DataType",
                table: "MixLanguageContent",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                collation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixConfigurationContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "MixConfigurationContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<string>(
                name: "DataType",
                table: "MixConfigurationContent",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                collation: "Vietnamese_CI_AS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "MixLanguageContent");

            migrationBuilder.DropColumn(
                name: "DataType",
                table: "MixLanguageContent");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "MixConfigurationContent");

            migrationBuilder.DropColumn(
                name: "DataType",
                table: "MixConfigurationContent");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixLanguageContent",
                type: "nvarchar(4000)",
                nullable: false,
                defaultValue: "",
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixConfigurationContent",
                type: "nvarchar(4000)",
                nullable: false,
                defaultValue: "",
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");
        }
    }
}

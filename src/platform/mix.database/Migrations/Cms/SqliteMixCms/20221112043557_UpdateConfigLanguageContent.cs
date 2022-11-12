using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations
{
    public partial class UpdateConfigLanguageContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixLanguageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldCollation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "MixLanguageContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "DataType",
                table: "MixLanguageContent",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixConfigurationContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldCollation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "MixConfigurationContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "DataType",
                table: "MixConfigurationContent",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE");
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
                type: "varchar(4000)",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixConfigurationContent",
                type: "varchar(4000)",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");
        }
    }
}

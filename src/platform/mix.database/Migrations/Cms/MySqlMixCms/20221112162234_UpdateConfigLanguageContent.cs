using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.MySqlMixCms
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
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "MixLanguageContent",
                type: "varchar(250)",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "DataType",
                table: "MixLanguageContent",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixConfigurationContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "MixConfigurationContent",
                type: "varchar(250)",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "DataType",
                table: "MixConfigurationContent",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");
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

            migrationBuilder.UpdateData(
                table: "MixLanguageContent",
                keyColumn: "DefaultContent",
                keyValue: null,
                column: "DefaultContent",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixLanguageContent",
                type: "varchar(4000)",
                nullable: false,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.UpdateData(
                table: "MixConfigurationContent",
                keyColumn: "DefaultContent",
                keyValue: null,
                column: "DefaultContent",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixConfigurationContent",
                type: "varchar(4000)",
                nullable: false,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8");
        }
    }
}

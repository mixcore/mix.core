using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class updattrfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DataId",
                table: "mix_module_attribute_value",
                maxLength: 50,
                nullable: false,
                defaultValueSql: "(N'')",
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "mix_module_attribute_value",
                type: "datetime",
                nullable: false,
                defaultValueSql: "('0001-01-01T00:00:00.0000000')",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "AttributeName",
                table: "mix_module_attribute_value",
                maxLength: 50,
                nullable: false,
                defaultValueSql: "(N'')",
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<bool>(
                name: "IsEncrypt",
                table: "mix_attribute_field",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequire",
                table: "mix_attribute_field",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSelect",
                table: "mix_attribute_field",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUnique",
                table: "mix_attribute_field",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEncrypt",
                table: "mix_attribute_field");

            migrationBuilder.DropColumn(
                name: "IsRequire",
                table: "mix_attribute_field");

            migrationBuilder.DropColumn(
                name: "IsSelect",
                table: "mix_attribute_field");

            migrationBuilder.DropColumn(
                name: "IsUnique",
                table: "mix_attribute_field");

            migrationBuilder.AlterColumn<string>(
                name: "DataId",
                table: "mix_module_attribute_value",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldDefaultValueSql: "(N'')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "mix_module_attribute_value",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "('0001-01-01T00:00:00.0000000')");

            migrationBuilder.AlterColumn<string>(
                name: "AttributeName",
                table: "mix_module_attribute_value",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldDefaultValueSql: "(N'')");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class addattrsetref : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<int>(
                name: "ReferenceId",
                table: "mix_attribute_field",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "mix_attribute_set_reference",
                columns: table => new
                {
                    SourceId = table.Column<int>(nullable: false),
                    DestinationId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 450, nullable: true),
                    Image = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_attribute_set_reference", x => new { x.SourceId, x.DestinationId });
                    table.ForeignKey(
                        name: "FK_mix_attribute_set_reference_mix_attribute_set1",
                        column: x => x.DestinationId,
                        principalTable: "mix_attribute_set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_attribute_set_reference_mix_attribute_set",
                        column: x => x.SourceId,
                        principalTable: "mix_attribute_set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_field_ReferenceId",
                table: "mix_attribute_field",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_set_reference_DestinationId",
                table: "mix_attribute_set_reference",
                column: "DestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_attribute_field_mix_attribute_set1",
                table: "mix_attribute_field",
                column: "ReferenceId",
                principalTable: "mix_attribute_set",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_attribute_field_mix_attribute_set1",
                table: "mix_attribute_field");

            migrationBuilder.DropTable(
                name: "mix_attribute_set_reference");

            migrationBuilder.DropIndex(
                name: "IX_mix_attribute_field_ReferenceId",
                table: "mix_attribute_field");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "mix_attribute_field");

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
        }
    }
}

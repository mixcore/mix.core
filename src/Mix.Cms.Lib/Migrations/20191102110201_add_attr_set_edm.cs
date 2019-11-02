using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class add_attr_set_edm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EdmAutoSend",
                table: "mix_attribute_set",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EdmFrom",
                table: "mix_attribute_set",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EdmSubject",
                table: "mix_attribute_set",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EdmTemplate",
                table: "mix_attribute_set",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormTemplate",
                table: "mix_attribute_set",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EdmAutoSend",
                table: "mix_attribute_set");

            migrationBuilder.DropColumn(
                name: "EdmFrom",
                table: "mix_attribute_set");

            migrationBuilder.DropColumn(
                name: "EdmSubject",
                table: "mix_attribute_set");

            migrationBuilder.DropColumn(
                name: "EdmTemplate",
                table: "mix_attribute_set");

            migrationBuilder.DropColumn(
                name: "FormTemplate",
                table: "mix_attribute_set");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class add_attr_value_set_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttributeSetName",
                table: "mix_attribute_set_value",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttributeSetName",
                table: "mix_attribute_field",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttributeSetName",
                table: "mix_attribute_set_value");

            migrationBuilder.DropColumn(
                name: "AttributeSetName",
                table: "mix_attribute_field");
        }
    }
}

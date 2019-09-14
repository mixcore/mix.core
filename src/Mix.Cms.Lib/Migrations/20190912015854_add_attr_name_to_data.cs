using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class add_attr_name_to_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AttributeName",
                table: "mix_attribute_set_value",
                newName: "AttributeFieldName");

            migrationBuilder.AddColumn<string>(
                name: "AttributeSetName",
                table: "mix_attribute_set_data",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttributeSetName",
                table: "mix_attribute_set_data");

            migrationBuilder.RenameColumn(
                name: "AttributeFieldName",
                table: "mix_attribute_set_value",
                newName: "AttributeName");
        }
    }
}

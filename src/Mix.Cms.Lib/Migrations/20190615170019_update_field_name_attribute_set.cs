using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class update_field_name_attribute_set : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SetAttributeId",
                table: "mix_article_attribute_data",
                newName: "AttributeSetId");

            migrationBuilder.AddColumn<int>(
                name: "AttributeSetId",
                table: "mix_page_attribute_data",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttributeSetId",
                table: "mix_page_attribute_data");

            migrationBuilder.RenameColumn(
                name: "AttributeSetId",
                table: "mix_article_attribute_data",
                newName: "SetAttributeId");
        }
    }
}

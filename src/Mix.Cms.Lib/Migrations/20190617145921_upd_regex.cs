using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class upd_regex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttributeFieldId",
                table: "mix_page_attribute_value",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AttributeFieldId",
                table: "mix_module_attribute_value",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Regex",
                table: "mix_module_attribute_value",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Regex",
                table: "mix_attribute_field",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttributeFieldId",
                table: "mix_article_attribute_value",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_attribute_value_AttributeFieldId",
                table: "mix_page_attribute_value",
                column: "AttributeFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_value_AttributeFieldId",
                table: "mix_module_attribute_value",
                column: "AttributeFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_article_attribute_value_AttributeFieldId",
                table: "mix_article_attribute_value",
                column: "AttributeFieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_article_attribute_value_mix_attribute_field",
                table: "mix_article_attribute_value",
                column: "AttributeFieldId",
                principalTable: "mix_attribute_field",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_module_attribute_value_mix_attribute_field",
                table: "mix_module_attribute_value",
                column: "AttributeFieldId",
                principalTable: "mix_attribute_field",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_page_attribute_value_mix_attribute_field",
                table: "mix_page_attribute_value",
                column: "AttributeFieldId",
                principalTable: "mix_attribute_field",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_article_attribute_value_mix_attribute_field",
                table: "mix_article_attribute_value");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_module_attribute_value_mix_attribute_field",
                table: "mix_module_attribute_value");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_page_attribute_value_mix_attribute_field",
                table: "mix_page_attribute_value");

            migrationBuilder.DropIndex(
                name: "IX_mix_page_attribute_value_AttributeFieldId",
                table: "mix_page_attribute_value");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_attribute_value_AttributeFieldId",
                table: "mix_module_attribute_value");

            migrationBuilder.DropIndex(
                name: "IX_mix_article_attribute_value_AttributeFieldId",
                table: "mix_article_attribute_value");

            migrationBuilder.DropColumn(
                name: "AttributeFieldId",
                table: "mix_page_attribute_value");

            migrationBuilder.DropColumn(
                name: "AttributeFieldId",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "Regex",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "Regex",
                table: "mix_attribute_field");

            migrationBuilder.DropColumn(
                name: "AttributeFieldId",
                table: "mix_article_attribute_value");
        }
    }
}

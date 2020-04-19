using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class aa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_related_attribute_data_mix_attribute_set_data2",
                table: "mix_related_attribute_data");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_mix_related_attribute_data_mix_attribute_set_data2",
                table: "mix_related_attribute_data",
                columns: new[] { "Id", "Specificulture" },
                principalTable: "mix_attribute_set_data",
                principalColumns: new[] { "Id", "Specificulture" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}

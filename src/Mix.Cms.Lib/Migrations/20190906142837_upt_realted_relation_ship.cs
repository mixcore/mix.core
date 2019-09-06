using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class upt_realted_relation_ship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_related_attribute_data_mix_attribute_set_data",
                table: "mix_related_attribute_data");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_related_attribute_data_mix_attribute_set_data1",
                table: "mix_related_attribute_data");

            migrationBuilder.DropIndex(
                name: "IX_mix_related_attribute_data_ParentId_Specificulture",
                table: "mix_related_attribute_data");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_related_attribute_data_mix_attribute_set_data2",
                table: "mix_related_attribute_data",
                columns: new[] { "Id", "Specificulture" },
                principalTable: "mix_attribute_set_data",
                principalColumns: new[] { "Id", "Specificulture" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_related_attribute_data_mix_attribute_set_data2",
                table: "mix_related_attribute_data");

            migrationBuilder.CreateIndex(
                name: "IX_mix_related_attribute_data_ParentId_Specificulture",
                table: "mix_related_attribute_data",
                columns: new[] { "ParentId", "Specificulture" });

            migrationBuilder.AddForeignKey(
                name: "FK_mix_related_attribute_data_mix_attribute_set_data",
                table: "mix_related_attribute_data",
                columns: new[] { "Id", "Specificulture" },
                principalTable: "mix_attribute_set_data",
                principalColumns: new[] { "Id", "Specificulture" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_related_attribute_data_mix_attribute_set_data1",
                table: "mix_related_attribute_data",
                columns: new[] { "ParentId", "Specificulture" },
                principalTable: "mix_attribute_set_data",
                principalColumns: new[] { "Id", "Specificulture" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}

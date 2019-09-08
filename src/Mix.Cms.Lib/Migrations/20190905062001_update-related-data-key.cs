using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class updaterelateddatakey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_related_attribute_data",
                table: "mix_related_attribute_data");

            migrationBuilder.DropIndex(
                name: "IX_mix_related_attribute_data_SourceId_Specificulture",
                table: "mix_related_attribute_data");

            migrationBuilder.RenameColumn(
                name: "DestinationId",
                table: "mix_related_attribute_data",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "mix_related_attribute_data",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_mix_related_attribute_data_DestinationId_Specificulture",
                table: "mix_related_attribute_data",
                newName: "IX_mix_related_attribute_data_ParentId_Specificulture");

            migrationBuilder.AlterColumn<int>(
                name: "ParentType",
                table: "mix_related_attribute_data",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_related_attribute_data_1",
                table: "mix_related_attribute_data",
                columns: new[] { "Id", "Specificulture", "ParentId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_related_attribute_data_1",
                table: "mix_related_attribute_data");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "mix_related_attribute_data",
                newName: "DestinationId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "mix_related_attribute_data",
                newName: "SourceId");

            migrationBuilder.RenameIndex(
                name: "IX_mix_related_attribute_data_ParentId_Specificulture",
                table: "mix_related_attribute_data",
                newName: "IX_mix_related_attribute_data_DestinationId_Specificulture");

            migrationBuilder.AlterColumn<int>(
                name: "ParentType",
                table: "mix_related_attribute_data",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_related_attribute_data",
                table: "mix_related_attribute_data",
                columns: new[] { "SourceId", "DestinationId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_related_attribute_data_SourceId_Specificulture",
                table: "mix_related_attribute_data",
                columns: new[] { "SourceId", "Specificulture" });
        }
    }
}

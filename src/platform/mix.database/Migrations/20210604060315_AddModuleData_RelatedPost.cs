using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Database.Migrations
{
    public partial class AddModuleData_RelatedPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MixPostContentId",
                table: "MixPostContent",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MixPostContent_MixPostContentId",
                table: "MixPostContent",
                column: "MixPostContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MixPostContent_MixPostContent_MixPostContentId",
                table: "MixPostContent",
                column: "MixPostContentId",
                principalTable: "MixPostContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixPostContent_MixPostContent_MixPostContentId",
                table: "MixPostContent");

            migrationBuilder.DropIndex(
                name: "IX_MixPostContent_MixPostContentId",
                table: "MixPostContent");

            migrationBuilder.DropColumn(
                name: "MixPostContentId",
                table: "MixPostContent");
        }
    }
}

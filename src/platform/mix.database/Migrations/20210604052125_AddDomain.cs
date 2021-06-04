using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Database.Migrations
{
    public partial class AddDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CssClass",
                table: "MixPostContent");

            migrationBuilder.DropColumn(
                name: "CssClass",
                table: "MixPageContent");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MixPageContent");

            migrationBuilder.DropColumn(
                name: "CssClass",
                table: "MixModuleContent");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MixModuleContent");

            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "MixPostContent",
                newName: "ClassName");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "MixPageContent",
                newName: "ClassName");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "MixModuleContent",
                newName: "ClassName");

            migrationBuilder.CreateTable(
                name: "MixDomain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MixSiteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDomain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDomain_MixSite_MixSiteId",
                        column: x => x.MixSiteId,
                        principalTable: "MixSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixDomain_MixSiteId",
                table: "MixDomain",
                column: "MixSiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MixDomain");

            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "MixPostContent",
                newName: "Icon");

            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "MixPageContent",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "MixModuleContent",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "CssClass",
                table: "MixPostContent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CssClass",
                table: "MixPageContent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixPageContent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CssClass",
                table: "MixModuleContent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MixModuleContent",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

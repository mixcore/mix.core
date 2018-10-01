using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class addrelatedaricle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mix_related_article",
                columns: table => new
                {
                    SourceId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    DestinationId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 450, nullable: true),
                    Image = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_related_article", x => new { x.SourceId, x.DestinationId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_related_article_mix_article1",
                        columns: x => new { x.DestinationId, x.Specificulture },
                        principalTable: "mix_article",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_related_article_mix_article",
                        columns: x => new { x.SourceId, x.Specificulture },
                        principalTable: "mix_article",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mix_related_article_DestinationId_Specificulture",
                table: "mix_related_article",
                columns: new[] { "DestinationId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_related_article_SourceId_Specificulture",
                table: "mix_related_article",
                columns: new[] { "SourceId", "Specificulture" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mix_related_article");
        }
    }
}

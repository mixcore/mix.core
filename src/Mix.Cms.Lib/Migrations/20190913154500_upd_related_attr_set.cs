using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class upd_related_attr_set : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_related_attribute_data_1",
                table: "mix_related_attribute_data");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_related_attribute_data_1",
                table: "mix_related_attribute_data",
                columns: new[] { "Id", "Specificulture", "ParentId", "ParentType" });

            migrationBuilder.CreateTable(
                name: "mix_related_attribute_set",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    ParentType = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 450, nullable: true),
                    Image = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_related_attribute_set", x => new { x.Id, x.Specificulture, x.ParentId, x.ParentType });
                    table.ForeignKey(
                        name: "FK_mix_related_attribute_set_mix_attribute_set",
                        column: x => x.Id,
                        principalTable: "mix_attribute_set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mix_related_attribute_set");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_related_attribute_data_1",
                table: "mix_related_attribute_data");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_related_attribute_data_1",
                table: "mix_related_attribute_data",
                columns: new[] { "Id", "Specificulture", "ParentId" });
        }
    }
}

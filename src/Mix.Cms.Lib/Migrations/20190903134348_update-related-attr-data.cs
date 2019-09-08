using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mix.Cms.Lib.Migrations
{
    public partial class updaterelatedattrdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_attribute_set_data_mix_attribute_set_data",
                table: "mix_attribute_set_data");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_attribute_set_value_mix_attribute_set_data",
                table: "mix_attribute_set_value");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_attribute_set_data",
                table: "mix_attribute_set_data");

            migrationBuilder.DropIndex(
                name: "IX_mix_attribute_set_data_ParentId",
                table: "mix_attribute_set_data");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "mix_attribute_set_data");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "mix_attribute_set_data");

            migrationBuilder.DropColumn(
                name: "ParentType",
                table: "mix_attribute_set_data");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_attribute_set_data",
                table: "mix_attribute_set_data",
                columns: new[] { "Id", "Specificulture" });

            migrationBuilder.CreateTable(
                name: "mix_related_attribute_data",
                columns: table => new
                {
                    SourceId = table.Column<string>(maxLength: 50, nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    DestinationId = table.Column<string>(maxLength: 50, nullable: false),
                    ParentType = table.Column<int>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 450, nullable: true),
                    Image = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_related_attribute_data", x => new { x.SourceId, x.DestinationId, x.Specificulture });
                    table.ForeignKey(
                        name: "FK_mix_related_attribute_data_mix_attribute_set_data1",
                        columns: x => new { x.DestinationId, x.Specificulture },
                        principalTable: "mix_attribute_set_data",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_related_attribute_data_mix_attribute_set_data",
                        columns: x => new { x.SourceId, x.Specificulture },
                        principalTable: "mix_attribute_set_data",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mix_related_attribute_data_DestinationId_Specificulture",
                table: "mix_related_attribute_data",
                columns: new[] { "DestinationId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_related_attribute_data_SourceId_Specificulture",
                table: "mix_related_attribute_data",
                columns: new[] { "SourceId", "Specificulture" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mix_related_attribute_data");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_attribute_set_data",
                table: "mix_attribute_set_data");

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "mix_attribute_set_data",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "mix_attribute_set_data",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentType",
                table: "mix_attribute_set_data",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_attribute_set_data",
                table: "mix_attribute_set_data",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_set_data_ParentId",
                table: "mix_attribute_set_data",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_attribute_set_data_mix_attribute_set_data",
                table: "mix_attribute_set_data",
                column: "ParentId",
                principalTable: "mix_attribute_set_data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_attribute_set_value_mix_attribute_set_data",
                table: "mix_attribute_set_value",
                column: "DataId",
                principalTable: "mix_attribute_set_data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

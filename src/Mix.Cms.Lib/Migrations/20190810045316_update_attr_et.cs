using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mix.Cms.Lib.Migrations
{
    public partial class update_attr_et : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_attribute_set_reference_mix_attribute_set1",
                table: "mix_attribute_set_reference");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_attribute_set_reference_mix_attribute_set",
                table: "mix_attribute_set_reference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_attribute_set_reference",
                table: "mix_attribute_set_reference");

            migrationBuilder.DropIndex(
                name: "IX_mix_attribute_set_reference_DestinationId",
                table: "mix_attribute_set_reference");

            migrationBuilder.RenameColumn(
                name: "DestinationId",
                table: "mix_attribute_set_reference",
                newName: "ParentType");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "mix_attribute_set_reference",
                newName: "ParentId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "mix_attribute_set_reference",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AttributeSetId",
                table: "mix_attribute_set_reference",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_attribute_set_reference",
                table: "mix_attribute_set_reference",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "mix_attribute_set_data",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    AttributeSetId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    ParentId = table.Column<string>(maxLength: 50, nullable: true),
                    ParentType = table.Column<int>(nullable: true),
                    ModuleId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_attribute_set_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_attribute_set_data_mix_attribute_set",
                        column: x => x.AttributeSetId,
                        principalTable: "mix_attribute_set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_attribute_set_data_mix_attribute_set_data",
                        column: x => x.ParentId,
                        principalTable: "mix_attribute_set_data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_attribute_set_value",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    AttributeFieldId = table.Column<int>(nullable: false),
                    Regex = table.Column<string>(maxLength: 250, nullable: true),
                    DataType = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    AttributeName = table.Column<string>(maxLength: 50, nullable: false),
                    BooleanValue = table.Column<bool>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataId = table.Column<string>(maxLength: 50, nullable: false),
                    DateTimeValue = table.Column<DateTime>(type: "datetime", nullable: true),
                    DoubleValue = table.Column<double>(nullable: true),
                    IntegerValue = table.Column<int>(nullable: true),
                    StringValue = table.Column<string>(maxLength: 4000, nullable: true),
                    EncryptValue = table.Column<string>(maxLength: 4000, nullable: true),
                    EncryptKey = table.Column<string>(maxLength: 50, nullable: true),
                    EncryptType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_attribute_set_value", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_attribute_set_value_mix_attribute_set_data",
                        column: x => x.DataId,
                        principalTable: "mix_attribute_set_data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_set_reference_AttributeSetId",
                table: "mix_attribute_set_reference",
                column: "AttributeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_set_data_AttributeSetId",
                table: "mix_attribute_set_data",
                column: "AttributeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_set_data_ParentId",
                table: "mix_attribute_set_data",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_set_value_DataId",
                table: "mix_attribute_set_value",
                column: "DataId");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_attribute_set_reference_mix_attribute_set",
                table: "mix_attribute_set_reference",
                column: "AttributeSetId",
                principalTable: "mix_attribute_set",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_attribute_set_reference_mix_attribute_set",
                table: "mix_attribute_set_reference");

            migrationBuilder.DropTable(
                name: "mix_attribute_set_value");

            migrationBuilder.DropTable(
                name: "mix_attribute_set_data");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_attribute_set_reference",
                table: "mix_attribute_set_reference");

            migrationBuilder.DropIndex(
                name: "IX_mix_attribute_set_reference_AttributeSetId",
                table: "mix_attribute_set_reference");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "mix_attribute_set_reference");

            migrationBuilder.DropColumn(
                name: "AttributeSetId",
                table: "mix_attribute_set_reference");

            migrationBuilder.RenameColumn(
                name: "ParentType",
                table: "mix_attribute_set_reference",
                newName: "DestinationId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "mix_attribute_set_reference",
                newName: "SourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_attribute_set_reference",
                table: "mix_attribute_set_reference",
                columns: new[] { "SourceId", "DestinationId" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_set_reference_DestinationId",
                table: "mix_attribute_set_reference",
                column: "DestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_attribute_set_reference_mix_attribute_set1",
                table: "mix_attribute_set_reference",
                column: "DestinationId",
                principalTable: "mix_attribute_set",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_attribute_set_reference_mix_attribute_set",
                table: "mix_attribute_set_reference",
                column: "SourceId",
                principalTable: "mix_attribute_set",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

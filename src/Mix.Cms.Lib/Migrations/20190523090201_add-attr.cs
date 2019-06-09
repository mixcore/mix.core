using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class addattr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_module_attribute_value_mix_module_attribute_set",
                table: "mix_module_attribute_value");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_module_attribute_value",
                table: "mix_module_attribute_value");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_attribute_value_AttributeSetId_ModuleId_Specificulture",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "AttributeSetId",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "DataType",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "mix_module_attribute_value");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "mix_module_attribute_value",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "AttributeName",
                table: "mix_module_attribute_value",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "BooleanValue",
                table: "mix_module_attribute_value",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "mix_module_attribute_value",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DataId",
                table: "mix_module_attribute_value",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeValue",
                table: "mix_module_attribute_value",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DoubleValue",
                table: "mix_module_attribute_value",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntegerValue",
                table: "mix_module_attribute_value",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringValue",
                table: "mix_module_attribute_value",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SetAttributeId",
                table: "mix_module",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_module_attribute_value",
                table: "mix_module_attribute_value",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "mix_article_attribute_data",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    ArticleId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Value = table.Column<string>(maxLength: 4000, nullable: true),
                    Fields = table.Column<string>(maxLength: 4000, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_article_attribute_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_article_attribute_data_mix_article",
                        columns: x => new { x.ArticleId, x.Specificulture },
                        principalTable: "mix_article",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_attribute_set",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 250, nullable: false),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_attribute_set", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_module_attribute_data",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Fields = table.Column<string>(maxLength: 4000, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_module_attribute_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_module_attribute_data_mix_module",
                        columns: x => new { x.ModuleId, x.Specificulture },
                        principalTable: "mix_module",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_attribute_data",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    PageId = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Value = table.Column<string>(maxLength: 4000, nullable: true),
                    Fields = table.Column<string>(maxLength: 4000, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_page_attribute_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_page_attribute_data_mix_page",
                        columns: x => new { x.PageId, x.Specificulture },
                        principalTable: "mix_page",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_article_attribute_value",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    DataId = table.Column<string>(maxLength: 50, nullable: false),
                    AttributeName = table.Column<string>(maxLength: 50, nullable: false),
                    ArticleId = table.Column<int>(nullable: false),
                    DoubleValue = table.Column<double>(nullable: true),
                    IntegerValue = table.Column<int>(nullable: true),
                    StringValue = table.Column<string>(maxLength: 4000, nullable: true),
                    DateTimeValue = table.Column<DateTime>(type: "datetime", nullable: true),
                    BooleanValue = table.Column<bool>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_article_attribute_value", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_article_attribute_value_mix_article_attribute_data",
                        column: x => x.DataId,
                        principalTable: "mix_article_attribute_data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_attribute_field",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    AttributeSetId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 250, nullable: true),
                    DataType = table.Column<int>(nullable: false),
                    DefaultValue = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Options = table.Column<string>(maxLength: 4000, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_attribute_field", x => new { x.Id, x.AttributeSetId });
                    table.ForeignKey(
                        name: "FK_mix_attribute_field_mix_attribute_set",
                        column: x => x.AttributeSetId,
                        principalTable: "mix_attribute_set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_attribute_value",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    DataId = table.Column<string>(maxLength: 50, nullable: false),
                    AttributeName = table.Column<string>(maxLength: 50, nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    PageId = table.Column<int>(nullable: false),
                    DoubleValue = table.Column<double>(nullable: true),
                    IntegerValue = table.Column<int>(nullable: true),
                    StringValue = table.Column<string>(maxLength: 4000, nullable: true),
                    DateTimeValue = table.Column<DateTime>(type: "datetime", nullable: true),
                    BooleanValue = table.Column<bool>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_page_attribute_value", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mix_page_attribute_value_mix_page_attribute_data",
                        column: x => x.DataId,
                        principalTable: "mix_page_attribute_data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_value_DataId",
                table: "mix_module_attribute_value",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_SetAttributeId",
                table: "mix_module",
                column: "SetAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_article_attribute_data_ArticleId_Specificulture",
                table: "mix_article_attribute_data",
                columns: new[] { "ArticleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_article_attribute_value_DataId",
                table: "mix_article_attribute_value",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_attribute_field_AttributeSetId",
                table: "mix_attribute_field",
                column: "AttributeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_data_ModuleId_Specificulture",
                table: "mix_module_attribute_data",
                columns: new[] { "ModuleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_attribute_data_PageId_Specificulture",
                table: "mix_page_attribute_data",
                columns: new[] { "PageId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_attribute_value_DataId",
                table: "mix_page_attribute_value",
                column: "DataId");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_article_mix_attribute_set",
                table: "mix_article",
                column: "SetAttributeId",
                principalTable: "mix_attribute_set",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_module_mix_attribute_set",
                table: "mix_module",
                column: "SetAttributeId",
                principalTable: "mix_attribute_set",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_module_attribute_value_mix_module_attribute_data",
                table: "mix_module_attribute_value",
                column: "DataId",
                principalTable: "mix_module_attribute_data",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_page_mix_attribute_set",
                table: "mix_page",
                column: "SetAttributeId",
                principalTable: "mix_attribute_set",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_article_mix_attribute_set",
                table: "mix_article");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_module_mix_attribute_set",
                table: "mix_module");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_module_attribute_value_mix_module_attribute_data",
                table: "mix_module_attribute_value");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_page_mix_attribute_set",
                table: "mix_page");

            migrationBuilder.DropTable(
                name: "mix_article_attribute_value");

            migrationBuilder.DropTable(
                name: "mix_attribute_field");

            migrationBuilder.DropTable(
                name: "mix_module_attribute_data");

            migrationBuilder.DropTable(
                name: "mix_page_attribute_value");

            migrationBuilder.DropTable(
                name: "mix_article_attribute_data");

            migrationBuilder.DropTable(
                name: "mix_attribute_set");

            migrationBuilder.DropTable(
                name: "mix_page_attribute_data");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_module_attribute_value",
                table: "mix_module_attribute_value");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_attribute_value_DataId",
                table: "mix_module_attribute_value");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_SetAttributeId",
                table: "mix_module");

            migrationBuilder.DropColumn(
                name: "AttributeName",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "BooleanValue",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "DataId",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "DateTimeValue",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "DoubleValue",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "IntegerValue",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "StringValue",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "SetAttributeId",
                table: "mix_module");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "mix_module_attribute_value",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<Guid>(
                name: "AttributeSetId",
                table: "mix_module_attribute_value",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "DataType",
                table: "mix_module_attribute_value",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "mix_module_attribute_value",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "mix_module_attribute_value",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "mix_module_attribute_value",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "mix_module_attribute_value",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_module_attribute_value",
                table: "mix_module_attribute_value",
                columns: new[] { "Id", "AttributeSetId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_value_AttributeSetId_ModuleId_Specificulture",
                table: "mix_module_attribute_value",
                columns: new[] { "AttributeSetId", "ModuleId", "Specificulture" });

            migrationBuilder.AddForeignKey(
                name: "FK_mix_module_attribute_value_mix_module_attribute_set",
                table: "mix_module_attribute_value",
                columns: new[] { "AttributeSetId", "ModuleId", "Specificulture" },
                principalTable: "mix_module_attribute_set",
                principalColumns: new[] { "Id", "ModuleId", "Specificulture" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}

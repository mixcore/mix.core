using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class updattr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_article_mix_attribute_set",
                table: "mix_article");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_module_mix_attribute_set",
                table: "mix_module");

            migrationBuilder.DropForeignKey(
                name: "FK_Mix_Module_Attribute_set_Mix_Module",
                table: "mix_module_attribute_set");

            migrationBuilder.DropForeignKey(
                name: "FK_Mix_Module_Attribute_set_Mix_Article_Module",
                table: "mix_module_attribute_set");

            migrationBuilder.DropForeignKey(
                name: "FK_Mix_Module_Attribute_set_Mix_Page_Module",
                table: "mix_module_attribute_set");

            migrationBuilder.DropForeignKey(
                name: "FK_Mix_Module_Data_Mix_Page_Module",
                table: "mix_module_data");

            migrationBuilder.DropForeignKey(
                name: "FK_Mix_Module_Data_Mix_Product_Module",
                table: "mix_module_data");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_page_mix_attribute_set",
                table: "mix_page");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_data_ModuleId_ArticleId_Specificulture",
                table: "mix_module_data");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_data_ModuleId_PageId_Specificulture",
                table: "mix_module_data");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_module_attribute_set",
                table: "mix_module_attribute_set");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_attribute_set_ModuleId_ArticleId_Specificulture",
                table: "mix_module_attribute_set");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_attribute_set_ModuleId_PageId_Specificulture",
                table: "mix_module_attribute_set");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_SetAttributeId",
                table: "mix_module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_attribute_field",
                table: "mix_attribute_field");

            migrationBuilder.DropColumn(
                name: "Fields",
                table: "mix_page_attribute_data");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "mix_page_attribute_data");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "mix_page_attribute_data");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "mix_module_data");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "mix_module_attribute_set");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "mix_module_attribute_set");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "mix_module_attribute_set");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "mix_module_attribute_set");

            migrationBuilder.DropColumn(
                name: "Fields",
                table: "mix_module_attribute_set");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "mix_module_attribute_set");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "mix_module_attribute_set");

            migrationBuilder.DropColumn(
                name: "Fields",
                table: "mix_module_attribute_data");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "mix_module_attribute_data");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "mix_module_attribute_data");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "mix_module_attribute_data");

            migrationBuilder.DropColumn(
                name: "SetAttributeId",
                table: "mix_module");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "mix_attribute_set");

            migrationBuilder.DropColumn(
                name: "Fields",
                table: "mix_article_attribute_data");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "mix_article_attribute_data");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "mix_article_attribute_data");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "mix_module_data",
                newName: "PageId");

            migrationBuilder.RenameIndex(
                name: "IX_mix_module_data_ModuleId_ProductId_Specificulture",
                table: "mix_module_data",
                newName: "IX_mix_module_data_ModuleId_PageId_Specificulture");

            migrationBuilder.AddColumn<int>(
                name: "DataType",
                table: "mix_module_attribute_value",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AttributeSetId",
                table: "mix_module_attribute_set",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "mix_module_attribute_set",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "mix_module_attribute_set",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttributeSetId",
                table: "mix_module_attribute_data",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "mix_attribute_set",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "DefaultValue",
                table: "mix_attribute_field",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "DataType",
                table: "mix_article_attribute_value",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_module_attribute_set_1",
                table: "mix_module_attribute_set",
                columns: new[] { "ModuleId", "Specificulture", "AttributeSetId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_attribute_field",
                table: "mix_attribute_field",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "mix_article_attribute_set",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    AttributeSetId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_article_attribute_set", x => new { x.ArticleId, x.Specificulture, x.AttributeSetId });
                    table.ForeignKey(
                        name: "FK_mix_article_attribute_set_mix_attribute_set",
                        column: x => x.AttributeSetId,
                        principalTable: "mix_attribute_set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_article_attribute_set_mix_article",
                        columns: x => new { x.ArticleId, x.Specificulture },
                        principalTable: "mix_article",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_page_attribute_set",
                columns: table => new
                {
                    PageId = table.Column<int>(nullable: false),
                    Specificulture = table.Column<string>(maxLength: 10, nullable: false),
                    AttributeSetId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Image = table.Column<string>(maxLength: 250, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_page_attribute_set", x => new { x.PageId, x.Specificulture, x.AttributeSetId });
                    table.ForeignKey(
                        name: "FK_mix_page_attribute_set_mix_attribute_set",
                        column: x => x.AttributeSetId,
                        principalTable: "mix_attribute_set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mix_page_attribute_set_mix_page",
                        columns: x => new { x.PageId, x.Specificulture },
                        principalTable: "mix_page",
                        principalColumns: new[] { "Id", "Specificulture" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_ArticleId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "ArticleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_PageId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "PageId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_set_AttributeSetId",
                table: "mix_module_attribute_set",
                column: "AttributeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_set_ModuleId_ArticleId_Specificulture",
                table: "mix_module_attribute_set",
                columns: new[] { "ModuleId", "AttributeSetId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_data_AttributeSetId",
                table: "mix_module_attribute_data",
                column: "AttributeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_article_attribute_set_AttributeSetId",
                table: "mix_article_attribute_set",
                column: "AttributeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_attribute_set_AttributeSetId",
                table: "mix_page_attribute_set",
                column: "AttributeSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_module_attribute_data_mix_attribute_set",
                table: "mix_module_attribute_data",
                column: "AttributeSetId",
                principalTable: "mix_attribute_set",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_module_attribute_set_mix_attribute_set",
                table: "mix_module_attribute_set",
                column: "AttributeSetId",
                principalTable: "mix_attribute_set",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_module_attribute_set_mix_module1",
                table: "mix_module_attribute_set",
                columns: new[] { "ModuleId", "Specificulture" },
                principalTable: "mix_module",
                principalColumns: new[] { "Id", "Specificulture" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mix_Module_Attribute_set_Mix_Article_Module",
                table: "mix_module_attribute_set",
                columns: new[] { "ModuleId", "AttributeSetId", "Specificulture" },
                principalTable: "mix_article_module",
                principalColumns: new[] { "ModuleId", "ArticleId", "Specificulture" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_module_data_mix_article",
                table: "mix_module_data",
                columns: new[] { "ArticleId", "Specificulture" },
                principalTable: "mix_article",
                principalColumns: new[] { "Id", "Specificulture" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_module_data_mix_page",
                table: "mix_module_data",
                columns: new[] { "PageId", "Specificulture" },
                principalTable: "mix_page",
                principalColumns: new[] { "Id", "Specificulture" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mix_Module_Data_Mix_Page_Module",
                table: "mix_module_data",
                columns: new[] { "ModuleId", "PageId", "Specificulture" },
                principalTable: "mix_page_module",
                principalColumns: new[] { "ModuleId", "CategoryId", "Specificulture" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_module_attribute_data_mix_attribute_set",
                table: "mix_module_attribute_data");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_module_attribute_set_mix_attribute_set",
                table: "mix_module_attribute_set");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_module_attribute_set_mix_module1",
                table: "mix_module_attribute_set");

            migrationBuilder.DropForeignKey(
                name: "FK_Mix_Module_Attribute_set_Mix_Article_Module",
                table: "mix_module_attribute_set");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_module_data_mix_article",
                table: "mix_module_data");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_module_data_mix_page",
                table: "mix_module_data");

            migrationBuilder.DropForeignKey(
                name: "FK_Mix_Module_Data_Mix_Page_Module",
                table: "mix_module_data");

            migrationBuilder.DropTable(
                name: "mix_article_attribute_set");

            migrationBuilder.DropTable(
                name: "mix_page_attribute_set");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_data_ArticleId_Specificulture",
                table: "mix_module_data");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_data_PageId_Specificulture",
                table: "mix_module_data");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_module_attribute_set_1",
                table: "mix_module_attribute_set");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_attribute_set_AttributeSetId",
                table: "mix_module_attribute_set");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_attribute_set_ModuleId_ArticleId_Specificulture",
                table: "mix_module_attribute_set");

            migrationBuilder.DropIndex(
                name: "IX_mix_module_attribute_data_AttributeSetId",
                table: "mix_module_attribute_data");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mix_attribute_field",
                table: "mix_attribute_field");

            migrationBuilder.DropColumn(
                name: "DataType",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "AttributeSetId",
                table: "mix_module_attribute_set");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "mix_module_attribute_set");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "mix_module_attribute_set");

            migrationBuilder.DropColumn(
                name: "AttributeSetId",
                table: "mix_module_attribute_data");

            migrationBuilder.DropColumn(
                name: "DataType",
                table: "mix_article_attribute_value");

            migrationBuilder.RenameColumn(
                name: "PageId",
                table: "mix_module_data",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_mix_module_data_ModuleId_PageId_Specificulture",
                table: "mix_module_data",
                newName: "IX_mix_module_data_ModuleId_ProductId_Specificulture");

            migrationBuilder.AddColumn<string>(
                name: "Fields",
                table: "mix_page_attribute_data",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "mix_page_attribute_data",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "mix_page_attribute_data",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "mix_module_data",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "mix_module_attribute_set",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "mix_module_attribute_set",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "mix_module_attribute_set",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "mix_module_attribute_set",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Fields",
                table: "mix_module_attribute_set",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "mix_module_attribute_set",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "mix_module_attribute_set",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fields",
                table: "mix_module_attribute_data",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "mix_module_attribute_data",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "mix_module_attribute_data",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "mix_module_attribute_data",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SetAttributeId",
                table: "mix_module",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "mix_attribute_set",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "mix_attribute_set",
                type: "datetime",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DefaultValue",
                table: "mix_attribute_field",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fields",
                table: "mix_article_attribute_data",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "mix_article_attribute_data",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "mix_article_attribute_data",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_module_attribute_set",
                table: "mix_module_attribute_set",
                columns: new[] { "Id", "ModuleId", "Specificulture" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_mix_attribute_field",
                table: "mix_attribute_field",
                columns: new[] { "Id", "AttributeSetId" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_ModuleId_ArticleId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "ModuleId", "ArticleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_data_ModuleId_PageId_Specificulture",
                table: "mix_module_data",
                columns: new[] { "ModuleId", "CategoryId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_set_ModuleId_ArticleId_Specificulture",
                table: "mix_module_attribute_set",
                columns: new[] { "ModuleId", "ArticleId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_attribute_set_ModuleId_PageId_Specificulture",
                table: "mix_module_attribute_set",
                columns: new[] { "ModuleId", "CategoryId", "Specificulture" });

            migrationBuilder.CreateIndex(
                name: "IX_mix_module_SetAttributeId",
                table: "mix_module",
                column: "SetAttributeId");

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
                name: "FK_Mix_Module_Attribute_set_Mix_Module",
                table: "mix_module_attribute_set",
                columns: new[] { "ModuleId", "Specificulture" },
                principalTable: "mix_module",
                principalColumns: new[] { "Id", "Specificulture" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mix_Module_Attribute_set_Mix_Article_Module",
                table: "mix_module_attribute_set",
                columns: new[] { "ModuleId", "ArticleId", "Specificulture" },
                principalTable: "mix_article_module",
                principalColumns: new[] { "ModuleId", "ArticleId", "Specificulture" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mix_Module_Attribute_set_Mix_Page_Module",
                table: "mix_module_attribute_set",
                columns: new[] { "ModuleId", "CategoryId", "Specificulture" },
                principalTable: "mix_page_module",
                principalColumns: new[] { "ModuleId", "CategoryId", "Specificulture" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mix_Module_Data_Mix_Page_Module",
                table: "mix_module_data",
                columns: new[] { "ModuleId", "CategoryId", "Specificulture" },
                principalTable: "mix_page_module",
                principalColumns: new[] { "ModuleId", "CategoryId", "Specificulture" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mix_Module_Data_Mix_Product_Module",
                table: "mix_module_data",
                columns: new[] { "ModuleId", "ProductId", "Specificulture" },
                principalTable: "mix_article_module",
                principalColumns: new[] { "ModuleId", "ArticleId", "Specificulture" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_page_mix_attribute_set",
                table: "mix_page",
                column: "SetAttributeId",
                principalTable: "mix_attribute_set",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

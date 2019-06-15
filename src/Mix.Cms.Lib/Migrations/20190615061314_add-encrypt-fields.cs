using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations
{
    public partial class addencryptfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_article_mix_set_attribute",
                table: "mix_article");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_page_mix_set_attribute",
                table: "mix_page");

            migrationBuilder.DropIndex(
                name: "IX_mix_page_SetAttributeId",
                table: "mix_page");

            migrationBuilder.DropIndex(
                name: "IX_mix_article_SetAttributeId",
                table: "mix_article");

            migrationBuilder.DropColumn(
                name: "SetAttributeData",
                table: "mix_page");

            migrationBuilder.DropColumn(
                name: "SetAttributeId",
                table: "mix_page");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "mix_article_attribute_data");

            migrationBuilder.DropColumn(
                name: "SetAttributeData",
                table: "mix_article");

            migrationBuilder.DropColumn(
                name: "SetAttributeId",
                table: "mix_article");

            migrationBuilder.AddColumn<string>(
                name: "EncryptKey",
                table: "mix_page_attribute_value",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EncryptType",
                table: "mix_page_attribute_value",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EncryptValue",
                table: "mix_page_attribute_value",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EncryptKey",
                table: "mix_module_attribute_value",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EncryptType",
                table: "mix_module_attribute_value",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EncryptValue",
                table: "mix_module_attribute_value",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EncryptKey",
                table: "mix_article_attribute_value",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EncryptType",
                table: "mix_article_attribute_value",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EncryptValue",
                table: "mix_article_attribute_value",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SetAttributeId",
                table: "mix_article_attribute_data",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncryptKey",
                table: "mix_page_attribute_value");

            migrationBuilder.DropColumn(
                name: "EncryptType",
                table: "mix_page_attribute_value");

            migrationBuilder.DropColumn(
                name: "EncryptValue",
                table: "mix_page_attribute_value");

            migrationBuilder.DropColumn(
                name: "EncryptKey",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "EncryptType",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "EncryptValue",
                table: "mix_module_attribute_value");

            migrationBuilder.DropColumn(
                name: "EncryptKey",
                table: "mix_article_attribute_value");

            migrationBuilder.DropColumn(
                name: "EncryptType",
                table: "mix_article_attribute_value");

            migrationBuilder.DropColumn(
                name: "EncryptValue",
                table: "mix_article_attribute_value");

            migrationBuilder.DropColumn(
                name: "SetAttributeId",
                table: "mix_article_attribute_data");

            migrationBuilder.AddColumn<string>(
                name: "SetAttributeData",
                table: "mix_page",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SetAttributeId",
                table: "mix_page",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "mix_article_attribute_data",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SetAttributeData",
                table: "mix_article",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SetAttributeId",
                table: "mix_article",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_mix_page_SetAttributeId",
                table: "mix_page",
                column: "SetAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_mix_article_SetAttributeId",
                table: "mix_article",
                column: "SetAttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_article_mix_set_attribute",
                table: "mix_article",
                column: "SetAttributeId",
                principalTable: "mix_set_attribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_page_mix_set_attribute",
                table: "mix_page",
                column: "SetAttributeId",
                principalTable: "mix_set_attribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Database.Migrations
{
    public partial class AddTheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MixUrlAlias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "MixUrlAlias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MixUrlAlias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemName",
                table: "MixUrlAlias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MixPost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "MixPost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MixPost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemName",
                table: "MixPost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MixPage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "MixPage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MixPage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemName",
                table: "MixPage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MixModule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "MixModule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MixModule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemName",
                table: "MixModule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MixLanguage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "MixLanguage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MixLanguage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemName",
                table: "MixLanguage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MixDomain",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MixDataContentId1",
                table: "MixDataContent",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MixDatabase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "MixCulture",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MixCulture",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemName",
                table: "MixCulture",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MixConfiguration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "MixConfiguration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "MixConfiguration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemName",
                table: "MixConfiguration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MixPostContent_MixDataContentId",
                table: "MixPostContent",
                column: "MixDataContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixPageContent_MixDataContentId",
                table: "MixPageContent",
                column: "MixDataContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixModuleContent_MixDataContentId",
                table: "MixModuleContent",
                column: "MixDataContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContent_MixDataContentId1",
                table: "MixDataContent",
                column: "MixDataContentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDataContent_MixDataContent_MixDataContentId1",
                table: "MixDataContent",
                column: "MixDataContentId1",
                principalTable: "MixDataContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MixModuleContent_MixDataContent_MixDataContentId",
                table: "MixModuleContent",
                column: "MixDataContentId",
                principalTable: "MixDataContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_MixPageContent_MixDataContent_MixDataContentId",
                table: "MixPageContent",
                column: "MixDataContentId",
                principalTable: "MixDataContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_MixPostContent_MixDataContent_MixDataContentId",
                table: "MixPostContent",
                column: "MixDataContentId",
                principalTable: "MixDataContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDataContent_MixDataContent_MixDataContentId1",
                table: "MixDataContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixModuleContent_MixDataContent_MixDataContentId",
                table: "MixModuleContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixPageContent_MixDataContent_MixDataContentId",
                table: "MixPageContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixPostContent_MixDataContent_MixDataContentId",
                table: "MixPostContent");

            migrationBuilder.DropIndex(
                name: "IX_MixPostContent_MixDataContentId",
                table: "MixPostContent");

            migrationBuilder.DropIndex(
                name: "IX_MixPageContent_MixDataContentId",
                table: "MixPageContent");

            migrationBuilder.DropIndex(
                name: "IX_MixModuleContent_MixDataContentId",
                table: "MixModuleContent");

            migrationBuilder.DropIndex(
                name: "IX_MixDataContent_MixDataContentId1",
                table: "MixDataContent");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MixUrlAlias");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "MixUrlAlias");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "MixUrlAlias");

            migrationBuilder.DropColumn(
                name: "SystemName",
                table: "MixUrlAlias");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MixPost");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "MixPost");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "MixPost");

            migrationBuilder.DropColumn(
                name: "SystemName",
                table: "MixPost");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MixPage");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "MixPage");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "MixPage");

            migrationBuilder.DropColumn(
                name: "SystemName",
                table: "MixPage");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MixModule");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "MixModule");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "MixModule");

            migrationBuilder.DropColumn(
                name: "SystemName",
                table: "MixModule");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MixLanguage");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "MixLanguage");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "MixLanguage");

            migrationBuilder.DropColumn(
                name: "SystemName",
                table: "MixLanguage");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "MixDomain");

            migrationBuilder.DropColumn(
                name: "MixDataContentId1",
                table: "MixDataContent");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "MixDatabase");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "MixCulture");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "MixCulture");

            migrationBuilder.DropColumn(
                name: "SystemName",
                table: "MixCulture");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MixConfiguration");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "MixConfiguration");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "MixConfiguration");

            migrationBuilder.DropColumn(
                name: "SystemName",
                table: "MixConfiguration");
        }
    }
}

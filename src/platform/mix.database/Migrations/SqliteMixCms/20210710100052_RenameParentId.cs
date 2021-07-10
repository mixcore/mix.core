using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class RenameParentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixConfigurationContent_MixConfiguration_MixConfigurationId",
                table: "MixConfigurationContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixDataContent_MixData_MixDataId1",
                table: "MixDataContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixLanguageContent_MixLanguage_MixLanguageId",
                table: "MixLanguageContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixModuleContent_MixModule_MixModuleId",
                table: "MixModuleContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixPageContent_MixPage_MixPageId",
                table: "MixPageContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixPostContent_MixPost_MixPostId",
                table: "MixPostContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixUrlAliasContent_MixUrlAlias_MixUrlAliasId",
                table: "MixUrlAliasContent");

            migrationBuilder.DropIndex(
                name: "IX_MixDataContent_MixDataId1",
                table: "MixDataContent");

            migrationBuilder.DropColumn(
                name: "MixDataId1",
                table: "MixDataContent");

            migrationBuilder.AlterColumn<int>(
                name: "MixUrlAliasId",
                table: "MixUrlAliasContent",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "MixUrlAliasContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MixPostId",
                table: "MixPostContent",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "MixPostContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MixPageId",
                table: "MixPageContent",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "MixPageContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MixModuleId",
                table: "MixModuleContent",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "MixModuleContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MixLanguageId",
                table: "MixLanguageContent",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "MixLanguageContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MixDataId",
                table: "MixDataContent",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "MixDataContent",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "MixConfigurationId",
                table: "MixConfigurationContent",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "MixConfigurationContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContent_MixDataId",
                table: "MixDataContent",
                column: "MixDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_MixConfigurationContent_MixConfiguration_MixConfigurationId",
                table: "MixConfigurationContent",
                column: "MixConfigurationId",
                principalTable: "MixConfiguration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MixDataContent_MixData_MixDataId",
                table: "MixDataContent",
                column: "MixDataId",
                principalTable: "MixData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MixLanguageContent_MixLanguage_MixLanguageId",
                table: "MixLanguageContent",
                column: "MixLanguageId",
                principalTable: "MixLanguage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MixModuleContent_MixModule_MixModuleId",
                table: "MixModuleContent",
                column: "MixModuleId",
                principalTable: "MixModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MixPageContent_MixPage_MixPageId",
                table: "MixPageContent",
                column: "MixPageId",
                principalTable: "MixPage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MixPostContent_MixPost_MixPostId",
                table: "MixPostContent",
                column: "MixPostId",
                principalTable: "MixPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MixUrlAliasContent_MixUrlAlias_MixUrlAliasId",
                table: "MixUrlAliasContent",
                column: "MixUrlAliasId",
                principalTable: "MixUrlAlias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixConfigurationContent_MixConfiguration_MixConfigurationId",
                table: "MixConfigurationContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixDataContent_MixData_MixDataId",
                table: "MixDataContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixLanguageContent_MixLanguage_MixLanguageId",
                table: "MixLanguageContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixModuleContent_MixModule_MixModuleId",
                table: "MixModuleContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixPageContent_MixPage_MixPageId",
                table: "MixPageContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixPostContent_MixPost_MixPostId",
                table: "MixPostContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixUrlAliasContent_MixUrlAlias_MixUrlAliasId",
                table: "MixUrlAliasContent");

            migrationBuilder.DropIndex(
                name: "IX_MixDataContent_MixDataId",
                table: "MixDataContent");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MixUrlAliasContent");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MixPostContent");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MixPageContent");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MixModuleContent");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MixLanguageContent");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MixDataContent");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MixConfigurationContent");

            migrationBuilder.AlterColumn<int>(
                name: "MixUrlAliasId",
                table: "MixUrlAliasContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MixPostId",
                table: "MixPostContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MixPageId",
                table: "MixPageContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MixModuleId",
                table: "MixModuleContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MixLanguageId",
                table: "MixLanguageContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MixDataId",
                table: "MixDataContent",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MixDataId1",
                table: "MixDataContent",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MixConfigurationId",
                table: "MixConfigurationContent",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContent_MixDataId1",
                table: "MixDataContent",
                column: "MixDataId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MixConfigurationContent_MixConfiguration_MixConfigurationId",
                table: "MixConfigurationContent",
                column: "MixConfigurationId",
                principalTable: "MixConfiguration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MixDataContent_MixData_MixDataId1",
                table: "MixDataContent",
                column: "MixDataId1",
                principalTable: "MixData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MixLanguageContent_MixLanguage_MixLanguageId",
                table: "MixLanguageContent",
                column: "MixLanguageId",
                principalTable: "MixLanguage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MixModuleContent_MixModule_MixModuleId",
                table: "MixModuleContent",
                column: "MixModuleId",
                principalTable: "MixModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MixPageContent_MixPage_MixPageId",
                table: "MixPageContent",
                column: "MixPageId",
                principalTable: "MixPage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MixPostContent_MixPost_MixPostId",
                table: "MixPostContent",
                column: "MixPostId",
                principalTable: "MixPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MixUrlAliasContent_MixUrlAlias_MixUrlAliasId",
                table: "MixUrlAliasContent",
                column: "MixUrlAliasId",
                principalTable: "MixUrlAlias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

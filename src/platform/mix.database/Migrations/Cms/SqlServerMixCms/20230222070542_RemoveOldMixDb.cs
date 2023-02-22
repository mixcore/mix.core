using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Cms.SqlServerMixCms
{
    /// <inheritdoc />
    public partial class RemoveOldMixDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixApplication_MixDataContent_MixDataContentId",
                table: "MixApplication");

            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ChildId",
                table: "MixDatabaseRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ParentId",
                table: "MixDatabaseRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_MixModuleContent_MixDataContent_MixDataContentId",
                table: "MixModuleContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixPageContent_MixDataContent_MixDataContentId",
                table: "MixPageContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixPostContent_MixDataContent_MixDataContentId",
                table: "MixPostContent");

            migrationBuilder.DropForeignKey(
                name: "FK_MixTheme_MixDataContent_MixDataContentId",
                table: "MixTheme");

            migrationBuilder.DropTable(
                name: "MixDataContentAssociation");

            migrationBuilder.DropTable(
                name: "MixDataContentValue");

            migrationBuilder.DropTable(
                name: "MixDataContent");

            migrationBuilder.DropTable(
                name: "MixData");

            migrationBuilder.DropIndex(
                name: "IX_MixTheme_MixDataContentId",
                table: "MixTheme");

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
                name: "IX_MixApplication_MixDataContentId",
                table: "MixApplication");

            migrationBuilder.DropColumn(
                name: "MixDataContentId",
                table: "MixTheme");

            migrationBuilder.DropColumn(
                name: "MixDataContentId",
                table: "MixPostContent");

            migrationBuilder.DropColumn(
                name: "MixDataContentId",
                table: "MixPageContent");

            migrationBuilder.DropColumn(
                name: "MixDataContentId",
                table: "MixModuleContent");

            migrationBuilder.DropColumn(
                name: "MixDataContentId",
                table: "MixApplication");

            migrationBuilder.AlterColumn<string>(
                name: "Styles",
                table: "MixViewTemplate",
                type: "ntext",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Scripts",
                table: "MixViewTemplate",
                type: "ntext",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixViewTemplate",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "MixThemeName",
                table: "MixViewTemplate",
                type: "varchar(50)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixViewTemplate",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "FolderType",
                table: "MixViewTemplate",
                type: "varchar(50)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MixViewTemplate",
                type: "varchar(50)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "FileFolder",
                table: "MixViewTemplate",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "MixViewTemplate",
                type: "varchar(50)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixViewTemplate",
                type: "ntext",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixUrlAlias",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixUrlAlias",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixUrlAlias",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixUrlAlias",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "TemplateFolder",
                table: "MixTheme",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixTheme",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PreviewUrl",
                table: "MixTheme",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTheme",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "MixTheme",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixTheme",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixTheme",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "AssetFolder",
                table: "MixTheme",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<int>(
                name: "MixDbId",
                table: "MixTheme",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixTenant",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixTenant",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryDomain",
                table: "MixTenant",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTenant",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixTenant",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixTenant",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixPostContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "MixPostContent",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixPostContent",
                type: "varchar(50)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixPostContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixPostContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixPostContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixPostContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixPostContent",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LayoutId",
                table: "MixPostContent",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPostContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixPostContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixPostContent",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixPostContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixPostContent",
                type: "ntext",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<int>(
                name: "MixDbId",
                table: "MixPostContent",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixPost",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPost",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixPageContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "MixPageContent",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixPageContent",
                type: "varchar(50)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixPageContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixPageContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixPageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixPageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixPageContent",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LayoutId",
                table: "MixPageContent",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPageContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixPageContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixPageContent",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixPageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixPageContent",
                type: "ntext",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<int>(
                name: "MixDbId",
                table: "MixPageContent",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixPage",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPage",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixPage",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixPage",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "MixModuleData",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MixModuleData",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixModuleData",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SimpleDataColumns",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDateTime",
                table: "MixModuleData",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixModuleData",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LayoutId",
                table: "MixModuleData",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "MixModuleData",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleData",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixModuleData",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "MixModuleData",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixModuleData",
                type: "ntext",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixModuleContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "MixModuleContent",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixModuleContent",
                type: "varchar(50)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixModuleContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixModuleContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixModuleContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixModuleContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixModuleContent",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LayoutId",
                table: "MixModuleContent",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixModuleContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixModuleContent",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixModuleContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixModuleContent",
                type: "ntext",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<int>(
                name: "MixDbId",
                table: "MixModuleContent",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MixModule",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixModule",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixModule",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModule",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixModule",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixModule",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixMedia",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "TargetUrl",
                table: "MixMedia",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "MixMedia",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixMedia",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixMedia",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixMedia",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                table: "MixMedia",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "FileProperties",
                table: "MixMedia",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MixMedia",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "FileFolder",
                table: "MixMedia",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "MixMedia",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixMedia",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixMedia",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixMedia",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(newid())",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixLanguageContent",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixLanguageContent",
                type: "varchar(50)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixLanguageContent",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguageContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixLanguageContent",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixLanguageContent",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixLanguageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixLanguageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "MixLanguageContent",
                type: "varchar(50)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixLanguageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "MixLanguageContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixLanguage",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixLanguage",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguage",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixLanguage",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixLanguage",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDomain",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDomain",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Host",
                table: "MixDomain",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDomain",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixDomain",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDiscussion",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDiscussion",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "MixDiscussion",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MixDatabaseRelationship",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDatabaseRelationship",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseRelationship",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixDatabaseContext",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDatabaseContext",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseContext",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDatabaseContext",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixDatabaseContext",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DatabaseProvider",
                table: "MixDatabaseContext",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixDatabaseColumn",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDatabaseColumn",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "MixDatabaseName",
                table: "MixDatabaseColumn",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseColumn",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDatabaseColumn",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultValue",
                table: "MixDatabaseColumn",
                type: "ntext",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "MixDatabaseColumn",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Configurations",
                table: "MixDatabaseColumn",
                type: "ntext",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDatabaseAssociation",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ParentDatabaseName",
                table: "MixDatabaseAssociation",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseAssociation",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "ChildDatabaseName",
                table: "MixDatabaseAssociation",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixDatabaseAssociation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(newid())",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatePermissions",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MixDatabase",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "ReadPermissions",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDatabase",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabase",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixDatabase",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DeletePermissions",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatePermissions",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixCulture",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixCulture",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Lcid",
                table: "MixCulture",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixCulture",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixCulture",
                type: "varchar(4000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixCulture",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixCulture",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "MixCulture",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixContributor",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixContributor",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "MixContributor",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixConfigurationContent",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixConfigurationContent",
                type: "varchar(50)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixConfigurationContent",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfigurationContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixConfigurationContent",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixConfigurationContent",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixConfigurationContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixConfigurationContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "MixConfigurationContent",
                type: "varchar(50)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixConfigurationContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "MixConfigurationContent",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixConfiguration",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixConfiguration",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfiguration",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixConfiguration",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixConfiguration",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixApplication",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixApplication",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixApplication",
                type: "varchar(250)",
                nullable: false,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixApplication",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AddColumn<int>(
                name: "MixDbId",
                table: "MixApplication",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ChildId",
                table: "MixDatabaseRelationship",
                column: "ChildId",
                principalTable: "MixDatabase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ParentId",
                table: "MixDatabaseRelationship",
                column: "ParentId",
                principalTable: "MixDatabase",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ChildId",
                table: "MixDatabaseRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ParentId",
                table: "MixDatabaseRelationship");

            migrationBuilder.DropColumn(
                name: "MixDbId",
                table: "MixTheme");

            migrationBuilder.DropColumn(
                name: "MixDbId",
                table: "MixPostContent");

            migrationBuilder.DropColumn(
                name: "MixDbId",
                table: "MixPageContent");

            migrationBuilder.DropColumn(
                name: "MixDbId",
                table: "MixModuleContent");

            migrationBuilder.DropColumn(
                name: "MixDbId",
                table: "MixApplication");

            migrationBuilder.AlterColumn<string>(
                name: "Styles",
                table: "MixViewTemplate",
                type: "ntext",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Scripts",
                table: "MixViewTemplate",
                type: "ntext",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixViewTemplate",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "MixThemeName",
                table: "MixViewTemplate",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixViewTemplate",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "FolderType",
                table: "MixViewTemplate",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MixViewTemplate",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "FileFolder",
                table: "MixViewTemplate",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "MixViewTemplate",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixViewTemplate",
                type: "ntext",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixUrlAlias",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixUrlAlias",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixUrlAlias",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixUrlAlias",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "TemplateFolder",
                table: "MixTheme",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixTheme",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "PreviewUrl",
                table: "MixTheme",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTheme",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "MixTheme",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixTheme",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixTheme",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "AssetFolder",
                table: "MixTheme",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AddColumn<Guid>(
                name: "MixDataContentId",
                table: "MixTheme",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixTenant",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixTenant",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryDomain",
                table: "MixTenant",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTenant",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixTenant",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixTenant",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixPostContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "MixPostContent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixPostContent",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixPostContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixPostContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixPostContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixPostContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixPostContent",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "LayoutId",
                table: "MixPostContent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPostContent",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixPostContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixPostContent",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixPostContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixPostContent",
                type: "ntext",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AddColumn<Guid>(
                name: "MixDataContentId",
                table: "MixPostContent",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixPost",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPost",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixPageContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "MixPageContent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixPageContent",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixPageContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixPageContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixPageContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixPageContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixPageContent",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "LayoutId",
                table: "MixPageContent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPageContent",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixPageContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixPageContent",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixPageContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixPageContent",
                type: "ntext",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AddColumn<Guid>(
                name: "MixDataContentId",
                table: "MixPageContent",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixPage",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPage",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixPage",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixPage",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "MixModuleData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "MixModuleData",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SimpleDataColumns",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDateTime",
                table: "MixModuleData",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixModuleData",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LayoutId",
                table: "MixModuleData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "MixModuleData",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleData",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "MixModuleData",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixModuleData",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixModuleContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "MixModuleContent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixModuleContent",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixModuleContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixModuleContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixModuleContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixModuleContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixModuleContent",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "LayoutId",
                table: "MixModuleContent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleContent",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixModuleContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixModuleContent",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixModuleContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixModuleContent",
                type: "ntext",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AddColumn<Guid>(
                name: "MixDataContentId",
                table: "MixModuleContent",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MixModule",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixModule",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixModule",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModule",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixModule",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixModule",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixMedia",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "TargetUrl",
                table: "MixMedia",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "MixMedia",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixMedia",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixMedia",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixMedia",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                table: "MixMedia",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "FileProperties",
                table: "MixMedia",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MixMedia",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "FileFolder",
                table: "MixMedia",
                type: "nvarchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "MixMedia",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixMedia",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixMedia",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixMedia",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "(newid())");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixLanguageContent",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixLanguageContent",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixLanguageContent",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguageContent",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixLanguageContent",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixLanguageContent",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixLanguageContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixLanguageContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "MixLanguageContent",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixLanguageContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "MixLanguageContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixLanguage",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixLanguage",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguage",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixLanguage",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixLanguage",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDomain",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDomain",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Host",
                table: "MixDomain",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDomain",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixDomain",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDiscussion",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDiscussion",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "MixDiscussion",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MixDatabaseRelationship",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDatabaseRelationship",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseRelationship",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixDatabaseContext",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDatabaseContext",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseContext",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDatabaseContext",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixDatabaseContext",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DatabaseProvider",
                table: "MixDatabaseContext",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixDatabaseColumn",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDatabaseColumn",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "MixDatabaseName",
                table: "MixDatabaseColumn",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseColumn",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDatabaseColumn",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultValue",
                table: "MixDatabaseColumn",
                type: "ntext",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "MixDatabaseColumn",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Configurations",
                table: "MixDatabaseColumn",
                type: "ntext",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDatabaseAssociation",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ParentDatabaseName",
                table: "MixDatabaseAssociation",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseAssociation",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "ChildDatabaseName",
                table: "MixDatabaseAssociation",
                type: "nvarchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixDatabaseAssociation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "(newid())");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatePermissions",
                table: "MixDatabase",
                type: "nvarchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MixDatabase",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixDatabase",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "ReadPermissions",
                table: "MixDatabase",
                type: "nvarchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixDatabase",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabase",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDatabase",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixDatabase",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DeletePermissions",
                table: "MixDatabase",
                type: "nvarchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatePermissions",
                table: "MixDatabase",
                type: "nvarchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixCulture",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixCulture",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Lcid",
                table: "MixCulture",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixCulture",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixCulture",
                type: "nvarchar(4000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixCulture",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixCulture",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "MixCulture",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixContributor",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixContributor",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "MixContributor",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixConfigurationContent",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixConfigurationContent",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixConfigurationContent",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfigurationContent",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixConfigurationContent",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixConfigurationContent",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixConfigurationContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixConfigurationContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "MixConfigurationContent",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixConfigurationContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "MixConfigurationContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixConfiguration",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixConfiguration",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfiguration",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixConfiguration",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixConfiguration",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixApplication",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixApplication",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixApplication",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixApplication",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AddColumn<Guid>(
                name: "MixDataContentId",
                table: "MixApplication",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MixData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    MixDatabaseId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    MixDatabaseName = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    MixTenantId = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixData_MixDatabase_MixDatabaseId",
                        column: x => x.MixDatabaseId,
                        principalTable: "MixDatabase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixDataContentAssociation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    MixCultureId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuidParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntParentId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    MixDatabaseId = table.Column<int>(type: "int", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MixTenantId = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentType = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Specificulture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDataContentAssociation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDataContentAssociation_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MixDataContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    MixCultureId = table.Column<int>(type: "int", nullable: false),
                    MixDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "ntext", nullable: true, collation: "Vietnamese_CI_AS"),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Excerpt = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    Icon = table.Column<string>(type: "nvarchar(50)", nullable: true, collation: "Vietnamese_CI_AS"),
                    Image = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    LayoutId = table.Column<int>(type: "int", nullable: true),
                    MixDatabaseId = table.Column<int>(type: "int", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MixTenantId = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    PublishedDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    SeoDescription = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    SeoKeywords = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    SeoName = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    SeoTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    Specificulture = table.Column<string>(type: "nvarchar(50)", nullable: false, collation: "Vietnamese_CI_AS"),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDataContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDataContent_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixDataContent_MixData_MixDataId",
                        column: x => x.MixDataId,
                        principalTable: "MixData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MixDataContentValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    MixCultureId = table.Column<int>(type: "int", nullable: false),
                    MixDatabaseColumnId = table.Column<int>(type: "int", nullable: false),
                    MixDataContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BooleanValue = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    DateTimeValue = table.Column<DateTime>(type: "datetime", nullable: true),
                    DoubleValue = table.Column<double>(type: "float", nullable: true),
                    EncryptKey = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    EncryptType = table.Column<string>(type: "nvarchar(50)", nullable: false, collation: "Vietnamese_CI_AS"),
                    EncryptValue = table.Column<string>(type: "ntext", nullable: true, collation: "Vietnamese_CI_AS"),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntegerValue = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    MixDatabaseColumnName = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    MixDatabaseId = table.Column<int>(type: "int", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    MixTenantId = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Specificulture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    StringValue = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDataContentValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDataContentValue_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixDataContentValue_MixDataContent_MixDataContentId",
                        column: x => x.MixDataContentId,
                        principalTable: "MixDataContent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MixDataContentValue_MixDatabaseColumn_MixDatabaseColumnId",
                        column: x => x.MixDatabaseColumnId,
                        principalTable: "MixDatabaseColumn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixTheme_MixDataContentId",
                table: "MixTheme",
                column: "MixDataContentId");

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
                name: "IX_MixApplication_MixDataContentId",
                table: "MixApplication",
                column: "MixDataContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixData_MixDatabaseId",
                table: "MixData",
                column: "MixDatabaseId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContent_MixCultureId",
                table: "MixDataContent",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContent_MixDataId",
                table: "MixDataContent",
                column: "MixDataId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContentAssociation_MixCultureId",
                table: "MixDataContentAssociation",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContentValue_MixCultureId",
                table: "MixDataContentValue",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContentValue_MixDatabaseColumnId",
                table: "MixDataContentValue",
                column: "MixDatabaseColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDataContentValue_MixDataContentId",
                table: "MixDataContentValue",
                column: "MixDataContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MixApplication_MixDataContent_MixDataContentId",
                table: "MixApplication",
                column: "MixDataContentId",
                principalTable: "MixDataContent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ChildId",
                table: "MixDatabaseRelationship",
                column: "ChildId",
                principalTable: "MixDatabase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ParentId",
                table: "MixDatabaseRelationship",
                column: "ParentId",
                principalTable: "MixDatabase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MixModuleContent_MixDataContent_MixDataContentId",
                table: "MixModuleContent",
                column: "MixDataContentId",
                principalTable: "MixDataContent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MixPageContent_MixDataContent_MixDataContentId",
                table: "MixPageContent",
                column: "MixDataContentId",
                principalTable: "MixDataContent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MixPostContent_MixDataContent_MixDataContentId",
                table: "MixPostContent",
                column: "MixDataContentId",
                principalTable: "MixDataContent",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MixTheme_MixDataContent_MixDataContentId",
                table: "MixTheme",
                column: "MixDataContentId",
                principalTable: "MixDataContent",
                principalColumn: "Id");
        }
    }
}

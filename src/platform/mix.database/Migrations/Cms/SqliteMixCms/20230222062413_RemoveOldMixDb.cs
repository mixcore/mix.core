using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Cms.SqliteMixCms
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

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixViewTemplate",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixUrlAlias",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTheme",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "MixDbId",
                table: "MixTheme",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTenant",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPostContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "MixDbId",
                table: "MixPostContent",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPost",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPageContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "MixDbId",
                table: "MixPageContent",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPage",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "MixModuleData",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MixModuleData",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixModuleData",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SimpleDataColumns",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDateTime",
                table: "MixModuleData",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixModuleData",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LayoutId",
                table: "MixModuleData",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "MixModuleData",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleData",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixModuleData",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "MixModuleData",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixModuleData",
                type: "ntext",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "MixDbId",
                table: "MixModuleContent",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModule",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixMedia",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguageContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguage",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDomain",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDiscussion",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseRelationship",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseContext",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseColumn",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseAssociation",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabase",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixCulture",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixContributor",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfigurationContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfiguration",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixApplication",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "MixDbId",
                table: "MixApplication",
                type: "INTEGER",
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

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixViewTemplate",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixUrlAlias",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTheme",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<Guid>(
                name: "MixDataContentId",
                table: "MixTheme",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTenant",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPostContent",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<Guid>(
                name: "MixDataContentId",
                table: "MixPostContent",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPost",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPageContent",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<Guid>(
                name: "MixDataContentId",
                table: "MixPageContent",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPage",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "MixModuleData",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "MixModuleData",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SimpleDataColumns",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDateTime",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "MixModuleData",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LayoutId",
                table: "MixModuleData",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleData",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "MixModuleData",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixModuleData",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleContent",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<Guid>(
                name: "MixDataContentId",
                table: "MixModuleContent",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModule",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixMedia",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguageContent",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguage",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDomain",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDiscussion",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseRelationship",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseContext",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseColumn",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseAssociation",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabase",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixCulture",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixContributor",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfigurationContent",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfiguration",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixApplication",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<Guid>(
                name: "MixDataContentId",
                table: "MixApplication",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MixData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "(newid())"),
                    MixDatabaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    MixDatabaseName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    MixTenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "(newid())"),
                    MixCultureId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataContentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GuidParentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Icon = table.Column<string>(type: "TEXT", nullable: true),
                    IntParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    MixDatabaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "TEXT", nullable: true),
                    MixTenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ParentType = table.Column<string>(type: "varchar(50)", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "TEXT", nullable: true),
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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "(newid())"),
                    MixCultureId = table.Column<int>(type: "INTEGER", nullable: false),
                    MixDataId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "ntext", nullable: true, collation: "NOCASE"),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Excerpt = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    Icon = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    Image = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    LayoutId = table.Column<int>(type: "integer", nullable: true),
                    MixDatabaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "TEXT", nullable: true),
                    MixTenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    PublishedDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    SeoDescription = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    SeoKeywords = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    SeoName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    SeoTitle = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    Specificulture = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    TemplateId = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE")
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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "(newid())"),
                    MixCultureId = table.Column<int>(type: "INTEGER", nullable: false),
                    MixDatabaseColumnId = table.Column<int>(type: "INTEGER", nullable: false),
                    MixDataContentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    BooleanValue = table.Column<bool>(type: "INTEGER", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataType = table.Column<string>(type: "varchar(50)", nullable: false),
                    DateTimeValue = table.Column<DateTime>(type: "datetime", nullable: true),
                    DoubleValue = table.Column<double>(type: "REAL", nullable: true),
                    EncryptKey = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    EncryptType = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE"),
                    EncryptValue = table.Column<string>(type: "ntext", nullable: true, collation: "NOCASE"),
                    Icon = table.Column<string>(type: "TEXT", nullable: true),
                    IntegerValue = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    MixDatabaseColumnName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    MixDatabaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    MixDatabaseName = table.Column<string>(type: "varchar(250)", nullable: true, collation: "NOCASE"),
                    MixTenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Specificulture = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    StringValue = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE")
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

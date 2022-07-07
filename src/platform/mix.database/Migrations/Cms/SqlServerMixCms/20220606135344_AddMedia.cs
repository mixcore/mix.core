using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqlServerMixCms
{
    public partial class AddMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MixThemeName",
                table: "MixViewTemplate",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "FolderType",
                table: "MixViewTemplate",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MixViewTemplate",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "FileFolder",
                table: "MixViewTemplate",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "MixViewTemplate",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixUrlAlias",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixUrlAlias",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "PreviewUrl",
                table: "MixTheme",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixTheme",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixTheme",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixTenant",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryDomain",
                table: "MixTenant",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixTenant",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixTenant",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixPostContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixPostContent",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixPostContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixPostContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixPostContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixPostContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixPostContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixPostContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixPageContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixPageContent",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixPageContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixPageContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixPageContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixPageContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixPageContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixPageContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixPage",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixPage",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixModuleContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixModuleContent",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixModuleContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixModuleContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixModuleContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixModuleContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixModuleContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixModuleContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

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
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixModule",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixModule",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixLanguageContent",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixLanguageContent",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixLanguageContent",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixLanguageContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixLanguageContent",
                type: "nvarchar(4000)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixLanguageContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixLanguage",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixLanguage",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixLanguage",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDomain",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixDomain",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "StringValue",
                table: "MixDataContentValue",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "MixDatabaseName",
                table: "MixDataContentValue",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "MixDatabaseColumnName",
                table: "MixDataContentValue",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "EncryptType",
                table: "MixDataContentValue",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "EncryptKey",
                table: "MixDataContentValue",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "MixDataContentValue",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "ParentType",
                table: "MixDataContentAssociation",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixDataContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixDataContent",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixDataContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixDataContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixDataContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixDataContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixDataContent",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixDataContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixDatabaseColumn",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "MixDatabaseName",
                table: "MixDatabaseColumn",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDatabaseColumn",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "MixDatabaseColumn",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

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
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDatabase",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixDatabase",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "MixDatabaseName",
                table: "MixData",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixCulture",
                type: "nvarchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Lcid",
                table: "MixCulture",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

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
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixCulture",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "MixCulture",
                type: "nvarchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixConfigurationContent",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixConfigurationContent",
                type: "nvarchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixConfigurationContent",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixConfigurationContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixConfigurationContent",
                type: "nvarchar(4000)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixConfigurationContent",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixConfiguration",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixConfiguration",
                type: "nvarchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixConfiguration",
                type: "nvarchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.CreateTable(
                name: "MixMedia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    Extension = table.Column<string>(type: "nvarchar(50)", nullable: true, collation: "Vietnamese_CI_AS"),
                    FileFolder = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(50)", nullable: true, collation: "Vietnamese_CI_AS"),
                    FileProperties = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(50)", nullable: true, collation: "Vietnamese_CI_AS"),
                    Title = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    Tags = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    Source = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    TargetUrl = table.Column<string>(type: "nvarchar(250)", nullable: true, collation: "Vietnamese_CI_AS"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(250)", nullable: false, collation: "Vietnamese_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(4000)", nullable: true, collation: "Vietnamese_CI_AS"),
                    MixTenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixMedia_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixMedia_MixTenantId",
                table: "MixMedia",
                column: "MixTenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MixMedia");

            migrationBuilder.AlterColumn<string>(
                name: "MixThemeName",
                table: "MixViewTemplate",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "FolderType",
                table: "MixViewTemplate",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MixViewTemplate",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "FileFolder",
                table: "MixViewTemplate",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "MixViewTemplate",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixUrlAlias",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixUrlAlias",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "PreviewUrl",
                table: "MixTheme",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixTheme",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixTheme",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixTenant",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryDomain",
                table: "MixTenant",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixTenant",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixTenant",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixPostContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixPostContent",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixPostContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixPostContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixPostContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixPostContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixPostContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixPostContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixPageContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixPageContent",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixPageContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixPageContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixPageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixPageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixPageContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixPageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixPage",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixPage",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixModuleContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixModuleContent",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixModuleContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixModuleContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixModuleContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixModuleContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixModuleContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixModuleContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

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
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixModule",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixModule",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixLanguageContent",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixLanguageContent",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixLanguageContent",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixLanguageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixLanguageContent",
                type: "varchar(4000)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixLanguageContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixLanguage",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixLanguage",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixLanguage",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDomain",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixDomain",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "StringValue",
                table: "MixDataContentValue",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "MixDatabaseName",
                table: "MixDataContentValue",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "MixDatabaseColumnName",
                table: "MixDataContentValue",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "EncryptType",
                table: "MixDataContentValue",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "EncryptKey",
                table: "MixDataContentValue",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "MixDataContentValue",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "ParentType",
                table: "MixDataContentAssociation",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixDataContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixDataContent",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixDataContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixDataContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixDataContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixDataContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixDataContent",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixDataContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixDatabaseColumn",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "MixDatabaseName",
                table: "MixDatabaseColumn",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDatabaseColumn",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "MixDatabaseColumn",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

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
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixDatabase",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "MixDatabaseName",
                table: "MixData",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixCulture",
                type: "varchar(50)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Lcid",
                table: "MixCulture",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

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
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixCulture",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Alias",
                table: "MixCulture",
                type: "varchar(250)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixConfigurationContent",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixConfigurationContent",
                type: "varchar(50)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixConfigurationContent",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixConfigurationContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DefaultContent",
                table: "MixConfigurationContent",
                type: "varchar(4000)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixConfigurationContent",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "SystemName",
                table: "MixConfiguration",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "MixConfiguration",
                type: "varchar(250)",
                nullable: false,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldCollation: "Vietnamese_CI_AS");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MixConfiguration",
                type: "varchar(4000)",
                nullable: true,
                collation: "Vietnamese_CI_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldNullable: true,
                oldCollation: "Vietnamese_CI_AS");
        }
    }
}

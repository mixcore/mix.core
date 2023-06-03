using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Cms.MySqlMixCms
{
    /// <inheritdoc />
    public partial class IncreaseFileNameLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixViewTemplate",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixUrlAlias",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTheme",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTenant",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPostContent",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPost",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPageContent",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPage",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleData",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleContent",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModule",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixMedia",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MixMedia",
                type: "varchar(250)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguageContent",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguage",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDomain",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDiscussion",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseRelationship",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseContext",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseColumn",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Configurations",
                table: "MixDatabaseColumn",
                type: "varchar(4000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseAssociation",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabase",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixCulture",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixContributor",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfigurationContent",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfiguration",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixApplication",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean(1)",
                oldDefaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixViewTemplate",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixUrlAlias",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTheme",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTenant",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPostContent",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPost",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPageContent",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPage",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleData",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleContent",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModule",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixMedia",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MixMedia",
                type: "varchar(50)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguageContent",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguage",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDomain",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDiscussion",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseRelationship",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseContext",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseColumn",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Configurations",
                table: "MixDatabaseColumn",
                type: "longtext",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseAssociation",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabase",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixCulture",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixContributor",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfigurationContent",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfiguration",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixApplication",
                type: "boolean(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");
        }
    }
}

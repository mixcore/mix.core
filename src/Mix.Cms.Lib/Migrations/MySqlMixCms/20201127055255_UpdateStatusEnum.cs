using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Cms.Lib.Migrations.MySqlMixCms
{
    public partial class UpdateStatusEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_url_alias",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_theme",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_template",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_related_post",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_related_data",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_related_attribute_set",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_related_attribute_data",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_post_module",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_post_media",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_post",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_portal_page_role",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_portal_page_navigation",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_portal_page",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_page_post",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_page_module",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_page",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_module_post",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_module_data",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_module",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_media",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_language",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_file",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_culture",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_configuration",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_cms_user",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_cache",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_attribute_set_value",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_attribute_set_reference",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_attribute_set_data",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_attribute_set",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_attribute_field",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "DataType",
                table: "mix_attribute_field",
                type: "varchar(30)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_url_alias",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_theme",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_template",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_related_post",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_related_data",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_related_attribute_set",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_related_attribute_data",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_post_module",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_post_media",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_post",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_portal_page_role",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_portal_page_navigation",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_portal_page",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_page_post",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_page_module",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_page",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_module_post",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_module_data",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_module",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_media",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_language",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_file",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_culture",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_configuration",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_cms_user",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_cache",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_attribute_set_value",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_attribute_set_reference",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_attribute_set_data",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_attribute_set",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "mix_attribute_field",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)")
                .Annotation("MySql:CharSet", "utf8")
                .Annotation("MySql:Collation", "utf8_unicode_ci")
                .OldAnnotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:Collation", "utf8_unicode_ci");

            migrationBuilder.AlterColumn<int>(
                name: "DataType",
                table: "mix_attribute_field",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)");
        }
    }
}

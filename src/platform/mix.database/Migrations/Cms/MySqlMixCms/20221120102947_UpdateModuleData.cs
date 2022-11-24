using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.MySqlMixCms
{
    public partial class UpdateModuleData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ChildId",
                table: "MixDatabaseRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ParentId",
                table: "MixDatabaseRelationship");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MixModuleData",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.UpdateData(
                table: "MixModuleData",
                keyColumn: "Specificulture",
                keyValue: null,
                column: "Specificulture",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixModuleData",
                type: "varchar(50)",
                nullable: false,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SimpleDataColumns",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDateTime",
                table: "MixModuleData",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "MixModuleData",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixModuleData",
                type: "varchar(50)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixModuleData",
                type: "varchar(4000)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "MixModuleData",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "MixModuleData",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ChildId",
                table: "MixDatabaseRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ParentId",
                table: "MixDatabaseRelationship");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "MixModuleData",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<string>(
                name: "Specificulture",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<string>(
                name: "SimpleDataColumns",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<string>(
                name: "SeoName",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<string>(
                name: "SeoKeywords",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<string>(
                name: "SeoDescription",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDateTime",
                table: "MixModuleData",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "MixModuleData",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<string>(
                name: "Excerpt",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDateTime",
                table: "MixModuleData",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixModuleData",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8");

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
        }
    }
}

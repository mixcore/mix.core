using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations
{
    public partial class AddMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StringValue",
                table: "MixDataContentValue",
                type: "varchar(4000)",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixDataContentValue",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "uuid()",
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "'uuid()'")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixDataContentAssociation",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "uuid()",
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "'uuid()'")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixDataContent",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "uuid()",
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "'uuid()'")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixData",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "uuid()",
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "'uuid()'")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "MixMedia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, defaultValueSql: "uuid()", collation: "ascii_general_ci"),
                    Extension = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FileFolder = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8"),
                    FileName = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FileProperties = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "varchar(50)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Title = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Tags = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Source = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    TargetUrl = table.Column<string>(type: "varchar(250)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "utf8_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8"),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                name: "StringValue",
                table: "MixDataContentValue",
                type: "text",
                nullable: true,
                collation: "utf8_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true,
                oldCollation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8")
                .OldAnnotation("MySql:CharSet", "utf8");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixDataContentValue",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "'uuid()'",
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "uuid()")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixDataContentAssociation",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "'uuid()'",
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "uuid()")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixDataContent",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "'uuid()'",
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "uuid()")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "MixData",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "'uuid()'",
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "uuid()")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
        }
    }
}

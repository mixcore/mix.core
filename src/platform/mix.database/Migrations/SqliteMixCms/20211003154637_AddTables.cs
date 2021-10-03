using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class AddTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MixUrlAliasContent");

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "MixUrlAlias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceContentGuidId",
                table: "MixUrlAlias",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceContentId",
                table: "MixUrlAlias",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "MixUrlAlias",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MixModuleData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fields = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    MixModuleId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Specificulture = table.Column<string>(type: "TEXT", nullable: true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    MixCultureId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Excerpt = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Layout = table.Column<string>(type: "TEXT", nullable: true),
                    Template = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<string>(type: "TEXT", nullable: true),
                    SeoDescription = table.Column<string>(type: "TEXT", nullable: true),
                    SeoKeywords = table.Column<string>(type: "TEXT", nullable: true),
                    SeoName = table.Column<string>(type: "TEXT", nullable: true),
                    SeoTitle = table.Column<string>(type: "TEXT", nullable: true),
                    PublishedDateTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixModuleData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixModuleData_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixModuleData_MixModule_MixModuleId",
                        column: x => x.MixModuleId,
                        principalTable: "MixModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixModuleData_MixCultureId",
                table: "MixModuleData",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixModuleData_MixModuleId",
                table: "MixModuleData",
                column: "MixModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MixModuleData");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "MixUrlAlias");

            migrationBuilder.DropColumn(
                name: "SourceContentGuidId",
                table: "MixUrlAlias");

            migrationBuilder.DropColumn(
                name: "SourceContentId",
                table: "MixUrlAlias");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "MixUrlAlias");

            migrationBuilder.CreateTable(
                name: "MixUrlAliasContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Alias = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    MixCultureId = table.Column<int>(type: "INTEGER", nullable: false),
                    MixUrlAliasId = table.Column<int>(type: "INTEGER", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    SourceId = table.Column<string>(type: "varchar(50)", nullable: true, collation: "NOCASE"),
                    Specificulture = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    SystemName = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixUrlAliasContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixUrlAliasContent_MixCulture_MixCultureId",
                        column: x => x.MixCultureId,
                        principalTable: "MixCulture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixUrlAliasContent_MixUrlAlias_MixUrlAliasId",
                        column: x => x.MixUrlAliasId,
                        principalTable: "MixUrlAlias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixUrlAliasContent_MixCultureId",
                table: "MixUrlAliasContent",
                column: "MixCultureId");

            migrationBuilder.CreateIndex(
                name: "IX_MixUrlAliasContent_MixUrlAliasId",
                table: "MixUrlAliasContent",
                column: "MixUrlAliasId");
        }
    }
}

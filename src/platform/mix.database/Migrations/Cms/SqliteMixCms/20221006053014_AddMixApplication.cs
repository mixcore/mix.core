using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class AddMixApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MixApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    BaseHref = table.Column<string>(type: "TEXT", nullable: true),
                    BaseRoute = table.Column<string>(type: "TEXT", nullable: true),
                    Domain = table.Column<string>(type: "TEXT", nullable: true),
                    BaseApiUrl = table.Column<string>(type: "TEXT", nullable: true),
                    TemplateId = table.Column<int>(type: "INTEGER", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: true),
                    MixDatabaseName = table.Column<string>(type: "TEXT", nullable: true),
                    MixDataContentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    MixTenantId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixApplication_MixDataContent_MixDataContentId",
                        column: x => x.MixDataContentId,
                        principalTable: "MixDataContent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MixApplication_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixApplication_MixDataContentId",
                table: "MixApplication",
                column: "MixDataContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MixApplication_MixTenantId",
                table: "MixApplication",
                column: "MixTenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MixApplication");
        }
    }
}

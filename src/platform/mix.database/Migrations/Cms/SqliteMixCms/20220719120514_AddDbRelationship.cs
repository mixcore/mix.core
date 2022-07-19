using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class AddDbRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MixDatabaseRelationship",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    MixTenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    LeftId = table.Column<int>(type: "INTEGER", nullable: false),
                    RightId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDatabaseRelationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDatabaseRelationship_MixDatabase_LeftId",
                        column: x => x.LeftId,
                        principalTable: "MixDatabase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixDatabaseRelationship_MixDatabase_RightId",
                        column: x => x.RightId,
                        principalTable: "MixDatabase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabaseRelationship_LeftId",
                table: "MixDatabaseRelationship",
                column: "LeftId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabaseRelationship_RightId",
                table: "MixDatabaseRelationship",
                column: "RightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MixDatabaseRelationship");
        }
    }
}

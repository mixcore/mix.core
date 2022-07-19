using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mix.Database.Migrations.PostgresqlMixCms
{
    public partial class AddDbRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WritePermissions",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)");

            migrationBuilder.AlterColumn<string>(
                name: "ReadPermissions",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)");

            migrationBuilder.CreateTable(
                name: "MixDatabaseRelationship",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    MixTenantId = table.Column<int>(type: "integer", nullable: false),
                    LeftId = table.Column<int>(type: "integer", nullable: false),
                    RightId = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.AlterColumn<string>(
                name: "WritePermissions",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReadPermissions",
                table: "MixDatabase",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);
        }
    }
}

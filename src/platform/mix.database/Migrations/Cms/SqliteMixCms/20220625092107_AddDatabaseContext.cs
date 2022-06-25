using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqliteMixCms
{
    public partial class AddDatabaseContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixDiscussion",
                type: "ntext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar");

            migrationBuilder.AddColumn<int>(
                name: "MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MixDatabaseContextDatabaseAssociation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    ModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    MixTenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    LeftId = table.Column<int>(type: "INTEGER", nullable: false),
                    RightId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDatabaseContextDatabaseAssociation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MixDatabaseContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DatabaseProvider = table.Column<string>(type: "varchar(50)", nullable: false),
                    ConnectionString = table.Column<string>(type: "varchar(250)", nullable: false),
                    MixDatabaseContextDatabaseAssociationId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(250)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "varchar(4000)", nullable: true, collation: "NOCASE"),
                    MixTenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    SystemName = table.Column<string>(type: "varchar(250)", nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDatabaseContext", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MixDatabaseContext_MixDatabaseContextDatabaseAssociation_MixDatabaseContextDatabaseAssociationId",
                        column: x => x.MixDatabaseContextDatabaseAssociationId,
                        principalTable: "MixDatabaseContextDatabaseAssociation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MixDatabaseContext_MixTenant_MixTenantId",
                        column: x => x.MixTenantId,
                        principalTable: "MixTenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabase_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase",
                column: "MixDatabaseContextDatabaseAssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabaseContext_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabaseContext",
                column: "MixDatabaseContextDatabaseAssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabaseContext_MixTenantId",
                table: "MixDatabaseContext",
                column: "MixTenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabase_MixDatabaseContextDatabaseAssociation_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase",
                column: "MixDatabaseContextDatabaseAssociationId",
                principalTable: "MixDatabaseContextDatabaseAssociation",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabase_MixDatabaseContextDatabaseAssociation_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase");

            migrationBuilder.DropTable(
                name: "MixDatabaseContext");

            migrationBuilder.DropTable(
                name: "MixDatabaseContextDatabaseAssociation");

            migrationBuilder.DropIndex(
                name: "IX_MixDatabase_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase");

            migrationBuilder.DropColumn(
                name: "MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MixDiscussion",
                type: "varchar",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "ntext");
        }
    }
}

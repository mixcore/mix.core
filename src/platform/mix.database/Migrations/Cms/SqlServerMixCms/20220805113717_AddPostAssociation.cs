using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqlServerMixCms
{
    public partial class AddPostAssociation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_LeftId",
                table: "MixDatabaseRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_RightId",
                table: "MixDatabaseRelationship");

            migrationBuilder.RenameColumn(
                name: "RightId",
                table: "MixPagePostAssociation",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "LeftId",
                table: "MixPagePostAssociation",
                newName: "ChildId");

            migrationBuilder.RenameColumn(
                name: "RightId",
                table: "MixPageModuleAssociation",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "LeftId",
                table: "MixPageModuleAssociation",
                newName: "ChildId");

            migrationBuilder.RenameColumn(
                name: "RightId",
                table: "MixModulePostAssociation",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "LeftId",
                table: "MixModulePostAssociation",
                newName: "ChildId");

            migrationBuilder.RenameColumn(
                name: "RightId",
                table: "MixDatabaseRelationship",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "LeftId",
                table: "MixDatabaseRelationship",
                newName: "ChildId");

            migrationBuilder.RenameIndex(
                name: "IX_MixDatabaseRelationship_RightId",
                table: "MixDatabaseRelationship",
                newName: "IX_MixDatabaseRelationship_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_MixDatabaseRelationship_LeftId",
                table: "MixDatabaseRelationship",
                newName: "IX_MixDatabaseRelationship_ChildId");

            migrationBuilder.RenameColumn(
                name: "RightId",
                table: "MixDatabaseContextDatabaseAssociation",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "LeftId",
                table: "MixDatabaseContextDatabaseAssociation",
                newName: "ChildId");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "MixDatabaseRelationship",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MixPostPostAssociation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MixTenantId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ChildId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixPostPostAssociation", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ChildId",
                table: "MixDatabaseRelationship");

            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_ParentId",
                table: "MixDatabaseRelationship");

            migrationBuilder.DropTable(
                name: "MixPostPostAssociation");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "MixDatabaseRelationship");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "MixPagePostAssociation",
                newName: "RightId");

            migrationBuilder.RenameColumn(
                name: "ChildId",
                table: "MixPagePostAssociation",
                newName: "LeftId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "MixPageModuleAssociation",
                newName: "RightId");

            migrationBuilder.RenameColumn(
                name: "ChildId",
                table: "MixPageModuleAssociation",
                newName: "LeftId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "MixModulePostAssociation",
                newName: "RightId");

            migrationBuilder.RenameColumn(
                name: "ChildId",
                table: "MixModulePostAssociation",
                newName: "LeftId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "MixDatabaseRelationship",
                newName: "RightId");

            migrationBuilder.RenameColumn(
                name: "ChildId",
                table: "MixDatabaseRelationship",
                newName: "LeftId");

            migrationBuilder.RenameIndex(
                name: "IX_MixDatabaseRelationship_ParentId",
                table: "MixDatabaseRelationship",
                newName: "IX_MixDatabaseRelationship_RightId");

            migrationBuilder.RenameIndex(
                name: "IX_MixDatabaseRelationship_ChildId",
                table: "MixDatabaseRelationship",
                newName: "IX_MixDatabaseRelationship_LeftId");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "MixDatabaseContextDatabaseAssociation",
                newName: "RightId");

            migrationBuilder.RenameColumn(
                name: "ChildId",
                table: "MixDatabaseContextDatabaseAssociation",
                newName: "LeftId");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_LeftId",
                table: "MixDatabaseRelationship",
                column: "LeftId",
                principalTable: "MixDatabase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabaseRelationship_MixDatabase_RightId",
                table: "MixDatabaseRelationship",
                column: "RightId",
                principalTable: "MixDatabase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

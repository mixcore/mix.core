using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Cms.MySqlMixCms
{
    /// <inheritdoc />
    public partial class UpdateMixDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabase_MixDatabaseContextDatabaseAssociation_MixDatabas~",
                table: "MixDatabase");

            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseContext_MixDatabaseContextDatabaseAssociation_Mix~",
                table: "MixDatabaseContext");

            migrationBuilder.DropTable(
                name: "MixDatabaseContextDatabaseAssociation");

            migrationBuilder.DropIndex(
                name: "IX_MixDatabaseContext_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabaseContext");

            migrationBuilder.DropIndex(
                name: "IX_MixDatabase_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase");

            migrationBuilder.DropColumn(
                name: "MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabaseContext");

            migrationBuilder.AddColumn<int?>(
                name: "MixDatabaseContextId",
                table: "MixDatabase",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Schema",
                table: "MixDatabaseContext",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Schema",
                table: "MixDatabaseContext");

            migrationBuilder.DropColumn(
                name: "MixDatabaseContextId",
                table: "MixDatabase");

            migrationBuilder.AddColumn<int>(
                name: "MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabaseContext",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MixDatabaseContextDatabaseAssociation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MixTenantId = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDatabaseContextDatabaseAssociation", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabaseContext_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabaseContext",
                column: "MixDatabaseContextDatabaseAssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabase_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase",
                column: "MixDatabaseContextDatabaseAssociationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabase_MixDatabaseContextDatabaseAssociation_MixDatabas~",
                table: "MixDatabase",
                column: "MixDatabaseContextDatabaseAssociationId",
                principalTable: "MixDatabaseContextDatabaseAssociation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabaseContext_MixDatabaseContextDatabaseAssociation_Mix~",
                table: "MixDatabaseContext",
                column: "MixDatabaseContextDatabaseAssociationId",
                principalTable: "MixDatabaseContextDatabaseAssociation",
                principalColumn: "Id");
        }
    }
}

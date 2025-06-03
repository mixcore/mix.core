using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Cms.MySql
{
    /// <inheritdoc />
    public partial class RefactorMixDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_database_mix_tenant_tenant_id",
                table: "mix_database");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_database_column_mix_database_mix_database_id",
                table: "mix_database_column");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_database_context_mix_tenant_tenant_id",
                table: "mix_database_context");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_database_relationship_mix_database_child_id",
                table: "mix_database_relationship");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_database_relationship_mix_database_parent_id",
                table: "mix_database_relationship");

            migrationBuilder.DropPrimaryKey(
                name: "pk_mix_database_relationship",
                table: "mix_database_relationship");

            migrationBuilder.DropPrimaryKey(
                name: "pk_mix_database_context",
                table: "mix_database_context");

            migrationBuilder.DropPrimaryKey(
                name: "pk_mix_database_column",
                table: "mix_database_column");

            migrationBuilder.DropPrimaryKey(
                name: "pk_mix_database",
                table: "mix_database");

            migrationBuilder.DropColumn(
                name: "child_database_name",
                table: "mix_database_association");

            migrationBuilder.DropColumn(
                name: "guid_child_id",
                table: "mix_database_association");

            migrationBuilder.DropColumn(
                name: "guid_parent_id",
                table: "mix_database_association");

            migrationBuilder.DropColumn(
                name: "parent_database_name",
                table: "mix_database_association");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "mix_database_association");

            migrationBuilder.RenameTable(
                name: "mix_database_relationship",
                newName: "mix_db_table_relationship");

            migrationBuilder.RenameTable(
                name: "mix_database_context",
                newName: "mix_db_database");

            migrationBuilder.RenameTable(
                name: "mix_database_column",
                newName: "mix_db_column");

            migrationBuilder.RenameTable(
                name: "mix_database",
                newName: "mix_db_table");

            migrationBuilder.RenameColumn(
                name: "mix_database_name",
                table: "mix_application",
                newName: "mix_db_table_name");

            migrationBuilder.RenameIndex(
                name: "IX_mix_database_relationship_parent_id",
                table: "mix_db_table_relationship",
                newName: "IX_mix_db_table_relationship_parent_id");

            migrationBuilder.RenameIndex(
                name: "IX_mix_database_relationship_child_id",
                table: "mix_db_table_relationship",
                newName: "IX_mix_db_table_relationship_child_id");

            migrationBuilder.RenameIndex(
                name: "IX_mix_database_context_tenant_id",
                table: "mix_db_database",
                newName: "IX_mix_db_database_tenant_id");

            migrationBuilder.RenameColumn(
                name: "mix_database_name",
                table: "mix_db_column",
                newName: "mix_db_table_name");

            migrationBuilder.RenameColumn(
                name: "mix_database_id",
                table: "mix_db_column",
                newName: "mix_db_table_id");

            migrationBuilder.RenameIndex(
                name: "IX_mix_database_column_mix_database_id",
                table: "mix_db_column",
                newName: "IX_mix_db_column_mix_db_table_id");

            migrationBuilder.RenameColumn(
                name: "mix_database_id",
                table: "mix_db_table",
                newName: "mix_db_table_id");

            migrationBuilder.RenameIndex(
                name: "IX_mix_database_tenant_id",
                table: "mix_db_table",
                newName: "IX_mix_db_table_tenant_id");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "mix_db_table_relationship",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "(uuid())")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_mix_db_table_relationship",
                table: "mix_db_table_relationship",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_mix_db_database",
                table: "mix_db_database",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_mix_db_column",
                table: "mix_db_column",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_mix_db_table",
                table: "mix_db_table",
                column: "id");

            migrationBuilder.CreateTable(
                name: "MixDbDataAssociation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    ParentDatabaseName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChildDatabaseName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GuidParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    GuidChildId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDbDataAssociation", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_db_column_mix_db_table_mix_db_table_id",
                table: "mix_db_column",
                column: "mix_db_table_id",
                principalTable: "mix_db_table",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_db_database_mix_tenant_tenant_id",
                table: "mix_db_database",
                column: "tenant_id",
                principalTable: "mix_tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_db_table_mix_tenant_tenant_id",
                table: "mix_db_table",
                column: "tenant_id",
                principalTable: "mix_tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_db_table_relationship_mix_db_table_child_id",
                table: "mix_db_table_relationship",
                column: "child_id",
                principalTable: "mix_db_table",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_db_table_relationship_mix_db_table_parent_id",
                table: "mix_db_table_relationship",
                column: "parent_id",
                principalTable: "mix_db_table",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mix_db_column_mix_db_table_mix_db_table_id",
                table: "mix_db_column");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_db_database_mix_tenant_tenant_id",
                table: "mix_db_database");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_db_table_mix_tenant_tenant_id",
                table: "mix_db_table");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_db_table_relationship_mix_db_table_child_id",
                table: "mix_db_table_relationship");

            migrationBuilder.DropForeignKey(
                name: "FK_mix_db_table_relationship_mix_db_table_parent_id",
                table: "mix_db_table_relationship");

            migrationBuilder.DropTable(
                name: "MixDbDataAssociation");

            migrationBuilder.DropPrimaryKey(
                name: "pk_mix_db_table_relationship",
                table: "mix_db_table_relationship");

            migrationBuilder.DropPrimaryKey(
                name: "pk_mix_db_table",
                table: "mix_db_table");

            migrationBuilder.DropPrimaryKey(
                name: "pk_mix_db_database",
                table: "mix_db_database");

            migrationBuilder.DropPrimaryKey(
                name: "pk_mix_db_column",
                table: "mix_db_column");

            migrationBuilder.RenameTable(
                name: "mix_db_table_relationship",
                newName: "mix_database_relationship");

            migrationBuilder.RenameTable(
                name: "mix_db_table",
                newName: "mix_database");

            migrationBuilder.RenameTable(
                name: "mix_db_database",
                newName: "mix_database_context");

            migrationBuilder.RenameTable(
                name: "mix_db_column",
                newName: "mix_database_column");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_name",
                table: "mix_application",
                newName: "mix_database_name");

            migrationBuilder.RenameIndex(
                name: "IX_mix_db_table_relationship_parent_id",
                table: "mix_database_relationship",
                newName: "IX_mix_database_relationship_parent_id");

            migrationBuilder.RenameIndex(
                name: "IX_mix_db_table_relationship_child_id",
                table: "mix_database_relationship",
                newName: "IX_mix_database_relationship_child_id");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_id",
                table: "mix_database",
                newName: "mix_database_id");

            migrationBuilder.RenameIndex(
                name: "IX_mix_db_table_tenant_id",
                table: "mix_database",
                newName: "IX_mix_database_tenant_id");

            migrationBuilder.RenameIndex(
                name: "IX_mix_db_database_tenant_id",
                table: "mix_database_context",
                newName: "IX_mix_database_context_tenant_id");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_name",
                table: "mix_database_column",
                newName: "mix_database_name");

            migrationBuilder.RenameColumn(
                name: "mix_db_table_id",
                table: "mix_database_column",
                newName: "mix_database_id");

            migrationBuilder.RenameIndex(
                name: "IX_mix_db_column_mix_db_table_id",
                table: "mix_database_column",
                newName: "IX_mix_database_column_mix_database_id");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "mix_database_relationship",
                type: "int",
                nullable: false,
                defaultValueSql: "(uuid())",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "child_database_name",
                table: "mix_database_relationship",
                type: "varchar(250)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<string>(
                name: "guid_child_id",
                table: "mix_database_relationship",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "guid_parent_id",
                table: "mix_database_relationship",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "parent_database_name",
                table: "mix_database_relationship",
                type: "varchar(250)",
                nullable: true,
                collation: "utf8_unicode_ci")
                .Annotation("MySql:CharSet", "utf8");

            migrationBuilder.AddColumn<int>(
                name: "tenant_id",
                table: "mix_database_relationship",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "pk_mix_database_relationship",
                table: "mix_database_relationship",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_mix_database",
                table: "mix_database",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_mix_database_context",
                table: "mix_database_context",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_mix_database_column",
                table: "mix_database_column",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_database_mix_tenant_tenant_id",
                table: "mix_database",
                column: "tenant_id",
                principalTable: "mix_tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_database_column_mix_database_mix_database_id",
                table: "mix_database_column",
                column: "mix_database_id",
                principalTable: "mix_database",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_database_context_mix_tenant_tenant_id",
                table: "mix_database_context",
                column: "tenant_id",
                principalTable: "mix_tenant",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mix_database_relationship_mix_database_child_id",
                table: "mix_database_relationship",
                column: "child_id",
                principalTable: "mix_database",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_mix_database_relationship_mix_database_parent_id",
                table: "mix_database_relationship",
                column: "parent_id",
                principalTable: "mix_database",
                principalColumn: "id");
        }
    }
}

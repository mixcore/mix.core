using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.Cms.SqliteMixCms
{
    /// <inheritdoc />
    public partial class UpdateMixDatabaseContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabase_MixDatabaseContextDatabaseAssociation_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase");

            migrationBuilder.DropForeignKey(
                name: "FK_MixDatabaseContext_MixDatabaseContextDatabaseAssociation_MixDatabaseContextDatabaseAssociationId",
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

            migrationBuilder.AddColumn<int>(
                name: "MixDatabaseContextId",
                table: "MixDatabase",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixViewTemplate",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixUrlAlias",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTheme",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTenant",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPostContent",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPost",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPageContent",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPage",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleData",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleContent",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModule",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixMedia",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MixMedia",
                type: "varchar(250)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguageContent",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguage",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDomain",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDiscussion",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseRelationship",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseContext",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "Scheme",
                table: "MixDatabaseContext",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseColumn",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Configurations",
                table: "MixDatabaseColumn",
                type: "varchar(4000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseAssociation",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<Guid>(
                name: "GuidChildId",
                table: "MixDatabaseAssociation",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GuidParentId",
                table: "MixDatabaseAssociation",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabase",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixCulture",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixContributor",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfigurationContent",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfiguration",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixApplication",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scheme",
                table: "MixDatabaseContext");

            migrationBuilder.DropColumn(
                name: "GuidChildId",
                table: "MixDatabaseAssociation");

            migrationBuilder.DropColumn(
                name: "GuidParentId",
                table: "MixDatabaseAssociation");

            migrationBuilder.DropColumn(
                name: "MixDatabaseContextId",
                table: "MixDatabase");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixViewTemplate",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixUrlAlias",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTheme",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixTenant",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPostContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPost",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPageContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixPage",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleData",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModuleContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixModule",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixMedia",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "MixMedia",
                type: "varchar(50)",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true,
                oldCollation: "NOCASE");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguageContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixLanguage",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDomain",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDiscussion",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseRelationship",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseContext",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabaseContext",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseColumn",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Configurations",
                table: "MixDatabaseColumn",
                type: "ntext",
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabaseAssociation",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixDatabase",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixCulture",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixContributor",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfigurationContent",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixConfiguration",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MixApplication",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "MixDatabaseContextDatabaseAssociation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChildId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MixTenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    ModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixDatabaseContextDatabaseAssociation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabaseContext_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabaseContext",
                column: "MixDatabaseContextDatabaseAssociationId");

            migrationBuilder.CreateIndex(
                name: "IX_MixDatabase_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase",
                column: "MixDatabaseContextDatabaseAssociationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabase_MixDatabaseContextDatabaseAssociation_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabase",
                column: "MixDatabaseContextDatabaseAssociationId",
                principalTable: "MixDatabaseContextDatabaseAssociation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MixDatabaseContext_MixDatabaseContextDatabaseAssociation_MixDatabaseContextDatabaseAssociationId",
                table: "MixDatabaseContext",
                column: "MixDatabaseContextDatabaseAssociationId",
                principalTable: "MixDatabaseContextDatabaseAssociation",
                principalColumn: "Id");
        }
    }
}

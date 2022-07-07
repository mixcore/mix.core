using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mix.Database.Migrations.SqliteAccount
{
    public partial class UpdateForeinKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_MixUsers_MixUserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_MixUsers_MixUserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_MixUsers_MixUserId1",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_MixUsers_MixUserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_MixUsers_MixUserId1",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_MixUsers_MixUserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserTokens_MixUserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_MixUserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_MixUserId1",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_MixUserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_MixUserId1",
                table: "AspNetUserLogins");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_MixUserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "MixUserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropColumn(
                name: "MixUserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "MixUserId1",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "MixUserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "MixUserId1",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "MixUserId",
                table: "AspNetUserClaims");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_MixUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "MixUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_MixUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "MixUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_MixUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "MixUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_MixUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "MixUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_MixUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_MixUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_MixUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_MixUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.AddColumn<Guid>(
                name: "MixUserId",
                table: "AspNetUserTokens",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MixUserId",
                table: "AspNetUserRoles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MixUserId1",
                table: "AspNetUserRoles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MixUserId",
                table: "AspNetUserLogins",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MixUserId1",
                table: "AspNetUserLogins",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MixUserId",
                table: "AspNetUserClaims",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserTokens_MixUserId",
                table: "AspNetUserTokens",
                column: "MixUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_MixUserId",
                table: "AspNetUserRoles",
                column: "MixUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_MixUserId1",
                table: "AspNetUserRoles",
                column: "MixUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_MixUserId",
                table: "AspNetUserLogins",
                column: "MixUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_MixUserId1",
                table: "AspNetUserLogins",
                column: "MixUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_MixUserId",
                table: "AspNetUserClaims",
                column: "MixUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_MixUsers_MixUserId",
                table: "AspNetUserClaims",
                column: "MixUserId",
                principalTable: "MixUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_MixUsers_MixUserId",
                table: "AspNetUserLogins",
                column: "MixUserId",
                principalTable: "MixUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_MixUsers_MixUserId1",
                table: "AspNetUserLogins",
                column: "MixUserId1",
                principalTable: "MixUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_MixUsers_MixUserId",
                table: "AspNetUserRoles",
                column: "MixUserId",
                principalTable: "MixUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_MixUsers_MixUserId1",
                table: "AspNetUserRoles",
                column: "MixUserId1",
                principalTable: "MixUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_MixUsers_MixUserId",
                table: "AspNetUserTokens",
                column: "MixUserId",
                principalTable: "MixUsers",
                principalColumn: "Id");
        }
    }
}

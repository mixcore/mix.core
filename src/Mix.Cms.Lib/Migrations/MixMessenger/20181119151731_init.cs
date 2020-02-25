using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mix.Cms.Messenger.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mix_messenger_hub_room",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Avatar = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(nullable: true),
                    HostId = table.Column<string>(maxLength: 128, nullable: true),
                    IsOpen = table.Column<bool>(nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    TeamId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_messenger_hub_room", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_messenger_team",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Avatar = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    HostId = table.Column<string>(maxLength: 128, nullable: true),
                    IsOpen = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_messenger_team", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_messenger_user",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    FacebookId = table.Column<string>(maxLength: 50, nullable: true),
                    Avatar = table.Column<string>(maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: false),
                    Status = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_messenger_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mix_messenger_user_device",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 50, nullable: false),
                    ConnectionId = table.Column<string>(maxLength: 50, nullable: false),
                    DeviceId = table.Column<string>(maxLength: 50, nullable: false),
                    Status = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_messenger_user_device", x => new { x.UserId, x.DeviceId });
                });

            migrationBuilder.CreateTable(
                name: "mix_messenger_message",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RoomId = table.Column<Guid>(nullable: true),
                    TeamId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_messenger_message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_messenger_message_messenger_hub_room",
                        column: x => x.RoomId,
                        principalTable: "mix_messenger_hub_room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_messenger_message_messenger_team",
                        column: x => x.TeamId,
                        principalTable: "mix_messenger_team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_messenger_message_messenger_user",
                        column: x => x.UserId,
                        principalTable: "mix_messenger_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_messenger_nav_room_user",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(maxLength: 50, nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_messenger_nav_room_user", x => new { x.RoomId, x.UserId });
                    table.ForeignKey(
                        name: "FK_messenger_nav_room_user_messenger_hub_room",
                        column: x => x.RoomId,
                        principalTable: "mix_messenger_hub_room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_messenger_nav_room_user_messenger_user",
                        column: x => x.UserId,
                        principalTable: "mix_messenger_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mix_messenger_nav_team_user",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(maxLength: 50, nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mix_messenger_nav_team_user", x => new { x.TeamId, x.UserId });
                    table.ForeignKey(
                        name: "FK_messenger_nav_team_user_messenger_team",
                        column: x => x.TeamId,
                        principalTable: "mix_messenger_team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_messenger_nav_team_user_messenger_user",
                        column: x => x.UserId,
                        principalTable: "mix_messenger_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_messenger_message_RoomId",
                table: "mix_messenger_message",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_messenger_message_TeamId",
                table: "mix_messenger_message",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_messenger_message_UserId",
                table: "mix_messenger_message",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_messenger_nav_room_user_UserId",
                table: "mix_messenger_nav_room_user",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_messenger_nav_team_user_UserId",
                table: "mix_messenger_nav_team_user",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mix_messenger_message");

            migrationBuilder.DropTable(
                name: "mix_messenger_nav_room_user");

            migrationBuilder.DropTable(
                name: "mix_messenger_nav_team_user");

            migrationBuilder.DropTable(
                name: "mix_messenger_user_device");

            migrationBuilder.DropTable(
                name: "mix_messenger_hub_room");

            migrationBuilder.DropTable(
                name: "mix_messenger_team");

            migrationBuilder.DropTable(
                name: "mix_messenger_user");
        }
    }
}
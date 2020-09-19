using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LBoard.Migrations
{
    public partial class LeaderboardTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKey");

            migrationBuilder.CreateTable(
                name: "Leaderboards",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaderboards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leaderboards_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leaderboards_OwnerId",
                table: "Leaderboards",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leaderboards");

            migrationBuilder.CreateTable(
                name: "ApiKey",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Key = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Owner = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKey", x => x.Id);
                });
        }
    }
}

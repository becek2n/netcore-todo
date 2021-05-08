using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TODO.Repository.Migrations
{
    public partial class Addedrefreshtokenstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TokenRefreshes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenRefreshes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenRefreshes_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$n3UDAa5uGQOhabXH6giK.Ol1HktRzqqU3fm87X3WmoftcY7TUdB1O");

            migrationBuilder.CreateIndex(
                name: "IX_TokenRefreshes_UserId",
                table: "TokenRefreshes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenRefreshes");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$tdMcp6u8dHAl4xysc05xcOOJpHoQJ/MhpQ58Aa4YKefmAtVTJgxj2");
        }
    }
}

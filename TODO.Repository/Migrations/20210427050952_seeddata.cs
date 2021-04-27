using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TODO.Repository.Migrations
{
    public partial class seeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    day = table.Column<DateTime>(type: "date", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true),
                    date_started = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    created_by = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    date_updated = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    password = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "tasks",
                columns: new[] { "id", "created_by", "date_updated", "day", "name", "status", "updated_by" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 1", false, null },
                    { 23, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 23", false, null },
                    { 24, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 24", false, null },
                    { 25, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 25", false, null },
                    { 26, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 26", false, null },
                    { 27, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 27", false, null },
                    { 28, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 28", false, null },
                    { 29, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 29", false, null },
                    { 22, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 22", false, null },
                    { 30, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 30", false, null },
                    { 32, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 32", false, null },
                    { 33, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 33", false, null },
                    { 34, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 34", false, null },
                    { 35, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 35", false, null },
                    { 36, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 36", false, null },
                    { 37, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 37", false, null },
                    { 38, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 38", false, null },
                    { 31, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 31", false, null },
                    { 21, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 21", false, null },
                    { 20, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 20", false, null },
                    { 19, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 19", false, null },
                    { 2, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 2", false, null },
                    { 3, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 3", false, null },
                    { 4, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 4", false, null },
                    { 5, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 5", false, null },
                    { 6, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 6", false, null },
                    { 7, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 7", false, null },
                    { 8, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 8", false, null },
                    { 9, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 9", false, null },
                    { 10, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 10", false, null },
                    { 11, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 11", false, null },
                    { 12, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 12", false, null },
                    { 13, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 13", false, null },
                    { 14, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 14", false, null },
                    { 15, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 15", false, null },
                    { 16, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 16", false, null },
                    { 17, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 17", false, null },
                    { 18, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 18", false, null },
                    { 39, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 39", false, null },
                    { 40, null, null, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "task 40", false, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

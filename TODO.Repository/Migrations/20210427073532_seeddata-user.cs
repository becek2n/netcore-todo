using Microsoft.EntityFrameworkCore.Migrations;

namespace TODO.Repository.Migrations
{
    public partial class seeddatauser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fullname",
                table: "users",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "fullname", "password", "username" },
                values: new object[] { 1, null, "$2a$11$/h6889L5wfCTOi9BOgKlmOQ8YOYndh4QTv0C50g20UYUm0U9pYY42", "test" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "fullname",
                table: "users");
        }
    }
}

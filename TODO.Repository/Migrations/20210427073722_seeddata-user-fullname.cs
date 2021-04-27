using Microsoft.EntityFrameworkCore.Migrations;

namespace TODO.Repository.Migrations
{
    public partial class seeddatauserfullname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "fullname", "password" },
                values: new object[] { "Testing User", "$2a$11$tdMcp6u8dHAl4xysc05xcOOJpHoQJ/MhpQ58Aa4YKefmAtVTJgxj2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "fullname", "password" },
                values: new object[] { null, "$2a$11$/h6889L5wfCTOi9BOgKlmOQ8YOYndh4QTv0C50g20UYUm0U9pYY42" });
        }
    }
}

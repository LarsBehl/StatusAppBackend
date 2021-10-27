using Microsoft.EntityFrameworkCore.Migrations;

namespace StatusAppBackend.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Key", "Name", "Url" },
                values: new object[,]
                {
                    { 1, "Steam Server Info", "https://api.steampowered.com/ISteamWebAPIUtil/GetServerInfo/v1/" },
                    { 2, "GitHub API", "https://api.github.com" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Key",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Key",
                keyValue: 2);
        }
    }
}

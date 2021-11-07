using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace StatusAppBackend.Migrations
{
    public partial class UserCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Salt = table.Column<byte[]>(type: "bytea", nullable: false),
                    Hash = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatedUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCreationTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<byte[]>(type: "bytea", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IssuerId = table.Column<int>(type: "integer", nullable: true),
                    CreatedUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCreationTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCreationTokens_Users_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "UserCreationTokens",
                columns: new[] { "Id", "CreatedUserId", "IssuedAt", "IssuerId", "Token" },
                values: new object[] { 1461171949, null, new DateTime(2021, 11, 7, 15, 24, 51, 268, DateTimeKind.Utc).AddTicks(3478), null, new byte[] { 253, 133, 118, 27, 21, 208, 180, 119 } });

            migrationBuilder.CreateIndex(
                name: "IX_UserCreationTokens_IssuerId",
                table: "UserCreationTokens",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedUserId",
                table: "Users",
                column: "CreatedUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserCreationTokens_CreatedUserId",
                table: "Users",
                column: "CreatedUserId",
                principalTable: "UserCreationTokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCreationTokens_Users_IssuerId",
                table: "UserCreationTokens");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserCreationTokens");
        }
    }
}

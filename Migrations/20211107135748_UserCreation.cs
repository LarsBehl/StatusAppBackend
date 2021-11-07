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
                name: "UserCreationToken",
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
                    table.PrimaryKey("PK_UserCreationToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
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
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_UserCreationToken_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "UserCreationToken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "UserCreationToken",
                columns: new[] { "Id", "CreatedUserId", "IssuedAt", "IssuerId", "Token" },
                values: new object[] { -1048121302, null, new DateTime(2021, 11, 7, 13, 57, 48, 552, DateTimeKind.Utc).AddTicks(5931), null, new byte[] { 218, 11, 250, 5, 220, 57, 248, 48 } });

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedUserId",
                table: "User",
                column: "CreatedUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCreationToken_IssuerId",
                table: "UserCreationToken",
                column: "IssuerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCreationToken_User_IssuerId",
                table: "UserCreationToken",
                column: "IssuerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_UserCreationToken_CreatedUserId",
                table: "User");

            migrationBuilder.DropTable(
                name: "UserCreationToken");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StatusAppBackend.Migrations
{
    public partial class TokenAsString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserCreationTokens",
                keyColumn: "Id",
                keyValue: 1461171949);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "UserCreationTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea");

            migrationBuilder.InsertData(
                table: "UserCreationTokens",
                columns: new[] { "Id", "CreatedUserId", "IssuedAt", "IssuerId", "Token" },
                values: new object[] { 1315379924, null, new DateTime(2021, 11, 7, 16, 34, 27, 821, DateTimeKind.Utc).AddTicks(6467), null, "AE2326D24A91A0BD" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserCreationTokens",
                keyColumn: "Id",
                keyValue: 1315379924);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Token",
                table: "UserCreationTokens",
                type: "bytea",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "UserCreationTokens",
                columns: new[] { "Id", "CreatedUserId", "IssuedAt", "IssuerId", "Token" },
                values: new object[] { 1461171949, null, new DateTime(2021, 11, 7, 15, 24, 51, 268, DateTimeKind.Utc).AddTicks(3478), null, new byte[] { 253, 133, 118, 27, 21, 208, 180, 119 } });
        }
    }
}

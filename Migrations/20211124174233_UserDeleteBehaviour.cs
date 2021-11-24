using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatusAppBackend.Migrations
{
    public partial class UserDeleteBehaviour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCreationTokens_Users_IssuerId",
                table: "UserCreationTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserCreationTokens_CreatedUserId",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "UserCreationTokens",
                keyColumn: "Id",
                keyValue: 1315379924);

            migrationBuilder.AlterColumn<DateTime>(
                name: "IssuedAt",
                table: "UserCreationTokens",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeRequested",
                table: "ServiceInformations",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCreationTokens_Users_IssuerId",
                table: "UserCreationTokens",
                column: "IssuerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserCreationTokens_CreatedUserId",
                table: "Users",
                column: "CreatedUserId",
                principalTable: "UserCreationTokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCreationTokens_Users_IssuerId",
                table: "UserCreationTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserCreationTokens_CreatedUserId",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IssuedAt",
                table: "UserCreationTokens",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeRequested",
                table: "ServiceInformations",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "UserCreationTokens",
                columns: new[] { "Id", "CreatedUserId", "IssuedAt", "IssuerId", "Token" },
                values: new object[] { 1315379924, null, new DateTime(2021, 11, 7, 16, 34, 27, 821, DateTimeKind.Utc).AddTicks(6467), null, "AE2326D24A91A0BD" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserCreationTokens_Users_IssuerId",
                table: "UserCreationTokens",
                column: "IssuerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserCreationTokens_CreatedUserId",
                table: "Users",
                column: "CreatedUserId",
                principalTable: "UserCreationTokens",
                principalColumn: "Id");
        }
    }
}

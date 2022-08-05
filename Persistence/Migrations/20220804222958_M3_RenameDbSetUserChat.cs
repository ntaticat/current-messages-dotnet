using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class M3_RenameDbSetUserChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersChats_Chats_ChatId",
                table: "UsersChats");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersChats_Users_UserId",
                table: "UsersChats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersChats",
                table: "UsersChats");

            migrationBuilder.RenameTable(
                name: "UsersChats",
                newName: "UserChat");

            migrationBuilder.RenameIndex(
                name: "IX_UsersChats_ChatId",
                table: "UserChat",
                newName: "IX_UserChat_ChatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserChat",
                table: "UserChat",
                columns: new[] { "UserId", "ChatId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserChat_Chats_ChatId",
                table: "UserChat",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "ChatId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChat_Users_UserId",
                table: "UserChat",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChat_Chats_ChatId",
                table: "UserChat");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChat_Users_UserId",
                table: "UserChat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserChat",
                table: "UserChat");

            migrationBuilder.RenameTable(
                name: "UserChat",
                newName: "UsersChats");

            migrationBuilder.RenameIndex(
                name: "IX_UserChat_ChatId",
                table: "UsersChats",
                newName: "IX_UsersChats_ChatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersChats",
                table: "UsersChats",
                columns: new[] { "UserId", "ChatId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersChats_Chats_ChatId",
                table: "UsersChats",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "ChatId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersChats_Users_UserId",
                table: "UsersChats",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

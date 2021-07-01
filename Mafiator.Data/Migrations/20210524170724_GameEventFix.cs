using Microsoft.EntityFrameworkCore.Migrations;

namespace Mafiator.Data.Migrations
{
    public partial class GameEventFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEvent_User",
                schema: "dbo",
                table: "GameEvent");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "dbo",
                table: "GameEvent",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_GameEvent_UserId",
                schema: "dbo",
                table: "GameEvent",
                newName: "IX_GameEvent_MemberId");

            migrationBuilder.AddColumn<bool>(
                name: "IsValidated",
                schema: "dbo",
                table: "GameEvent",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_GameEvent_Member",
                schema: "dbo",
                table: "GameEvent",
                column: "MemberId",
                principalSchema: "dbo",
                principalTable: "GameMember",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEvent_Member",
                schema: "dbo",
                table: "GameEvent");

            migrationBuilder.DropColumn(
                name: "IsValidated",
                schema: "dbo",
                table: "GameEvent");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                schema: "dbo",
                table: "GameEvent",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GameEvent_MemberId",
                schema: "dbo",
                table: "GameEvent",
                newName: "IX_GameEvent_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEvent_User",
                schema: "dbo",
                table: "GameEvent",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

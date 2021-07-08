using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mafiator.Data.Migrations
{
    public partial class chat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatMessage",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    MessageType = table.Column<short>(type: "smallint", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getDate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getDate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessage_Room",
                        column: x => x.RoomId,
                        principalSchema: "dbo",
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessage_User",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reaction",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getDate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getDate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reaction", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountryCode",
                schema: "dbo",
                table: "Users",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_RoomId",
                schema: "dbo",
                table: "ChatMessage",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_UserId",
                schema: "dbo",
                table: "ChatMessage",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Reaction",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Users_CountryCode",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                schema: "dbo",
                table: "Users");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mafiator.Data.Migrations
{
    public partial class VoteMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vote",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    GameId = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    VoterId = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    TargetId = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getDate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getDate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Target_Target",
                        column: x => x.TargetId,
                        principalSchema: "dbo",
                        principalTable: "GameMember",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vote_Game",
                        column: x => x.GameId,
                        principalSchema: "dbo",
                        principalTable: "Game",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Voter_Voter",
                        column: x => x.VoterId,
                        principalSchema: "dbo",
                        principalTable: "GameMember",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vote_GameId",
                schema: "dbo",
                table: "Vote",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_TargetId",
                schema: "dbo",
                table: "Vote",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_VoterId",
                schema: "dbo",
                table: "Vote",
                column: "VoterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vote",
                schema: "dbo");
        }
    }
}

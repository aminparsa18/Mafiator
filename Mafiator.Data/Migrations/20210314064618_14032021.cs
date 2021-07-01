using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mafiator.Data.Migrations
{
    public partial class _14032021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                schema: "dbo",
                table: "Room");

            migrationBuilder.CreateTable(
                name: "Gem",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getDate())"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getDate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gem", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "dbo",
                table: "Users",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gem",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                schema: "dbo",
                table: "Room",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

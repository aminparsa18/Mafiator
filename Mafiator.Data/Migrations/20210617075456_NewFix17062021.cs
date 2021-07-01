using Microsoft.EntityFrameworkCore.Migrations;

namespace Mafiator.Data.Migrations
{
    public partial class NewFix17062021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "dbo",
                table: "Room",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Room_Country",
                schema: "dbo",
                table: "Room",
                column: "Country");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Room_Country",
                schema: "dbo",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "Country",
                schema: "dbo",
                table: "Room");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

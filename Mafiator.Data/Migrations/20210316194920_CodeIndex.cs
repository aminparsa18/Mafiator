using Microsoft.EntityFrameworkCore.Migrations;

namespace Mafiator.Data.Migrations
{
    public partial class CodeIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Code",
                schema: "dbo",
                table: "Users",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Code",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "dbo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "dbo",
                table: "Users",
                column: "UserName");
        }
    }
}

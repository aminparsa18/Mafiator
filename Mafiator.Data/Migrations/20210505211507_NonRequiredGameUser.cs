using Microsoft.EntityFrameworkCore.Migrations;

namespace Mafiator.Data.Migrations
{
    public partial class NonRequiredGameUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "GameMember",
                type: "nvarchar(26)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(26)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "dbo",
                table: "GameMember",
                type: "nvarchar(26)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(26)",
                oldNullable: true);
        }
    }
}

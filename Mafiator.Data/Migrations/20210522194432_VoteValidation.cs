using Microsoft.EntityFrameworkCore.Migrations;

namespace Mafiator.Data.Migrations
{
    public partial class VoteValidation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValidated",
                schema: "dbo",
                table: "Vote",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValidated",
                schema: "dbo",
                table: "Vote");
        }
    }
}

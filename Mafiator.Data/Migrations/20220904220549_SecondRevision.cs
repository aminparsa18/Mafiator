using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mafiator.Data.Migrations;

public partial class SecondRevision : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Token",
            schema: "dbo",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "PersianCaption",
            schema: "dbo",
            table: "Roles");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Token",
            schema: "dbo",
            table: "Users",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PersianCaption",
            schema: "dbo",
            table: "Roles",
            type: "nvarchar(max)",
            nullable: true);
    }
}

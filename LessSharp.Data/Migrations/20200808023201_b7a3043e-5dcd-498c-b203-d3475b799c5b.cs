using Microsoft.EntityFrameworkCore.Migrations;

namespace LessSharp.Data.Migrations
{
    public partial class b7a3043e5dcd498cb203d3475b799c5b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_invalid",
                table: "sys_token");

            migrationBuilder.AddColumn<bool>(
                name: "is_disabled",
                table: "sys_token",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_disabled",
                table: "sys_token");

            migrationBuilder.AddColumn<bool>(
                name: "is_invalid",
                table: "sys_token",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

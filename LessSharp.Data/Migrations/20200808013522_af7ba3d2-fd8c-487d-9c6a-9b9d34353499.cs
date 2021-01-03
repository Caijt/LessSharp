using Microsoft.EntityFrameworkCore.Migrations;

namespace LessSharp.Data.Migrations
{
    public partial class af7ba3d2fd8c487d9c6a9b9d34353499 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_invalid",
                table: "sys_token",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_invalid",
                table: "sys_token");
        }
    }
}

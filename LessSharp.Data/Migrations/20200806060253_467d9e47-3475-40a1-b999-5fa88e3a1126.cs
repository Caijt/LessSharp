using Microsoft.EntityFrameworkCore.Migrations;

namespace LessSharp.Data.Migrations
{
    public partial class _467d9e47347540a1b9995fa88e3a1126 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "sys_menu",
                columns: new[] { "id", "can_multiple_open", "has_read", "has_review", "has_write", "icon", "is_page", "name", "order", "parent_id", "parent_ids", "path" },
                values: new object[] { 7, false, false, false, false, null, false, "Token管理", 6, 1, "1", "token" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 7);
        }
    }
}

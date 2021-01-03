using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LessSharp.Data.Migrations
{
    public partial class _8edf4b53a0cc4b7ea9c4682d6e0c69e9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sys_token",
                columns: table => new
                {
                    access_token = table.Column<string>(nullable: false),
                    access_expire = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    ip = table.Column<string>(nullable: true),
                    refresh_token = table.Column<string>(nullable: true),
                    refresh_expire = table.Column<DateTime>(nullable: false),
                    create_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_token", x => x.access_token);
                    table.ForeignKey(
                        name: "FK_sys_token_sys_user_user_id",
                        column: x => x.user_id,
                        principalTable: "sys_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "sys_menu",
                columns: new[] { "id", "can_multiple_open", "has_read", "has_review", "has_write", "icon", "is_page", "name", "order", "parent_id", "parent_ids", "path" },
                values: new object[] { 6, false, false, false, false, null, false, "配置管理", 5, 1, "1", "config" });

            migrationBuilder.CreateIndex(
                name: "IX_sys_token_user_id",
                table: "sys_token",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sys_token");

            migrationBuilder.DeleteData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 6);
        }
    }
}

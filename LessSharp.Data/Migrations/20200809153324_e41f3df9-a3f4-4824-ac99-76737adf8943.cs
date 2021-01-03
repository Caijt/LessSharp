using Microsoft.EntityFrameworkCore.Migrations;

namespace LessSharp.Data.Migrations
{
    public partial class e41f3df9a3f44824ac9976737adf8943 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "sys_user_login_log",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_sys_user_login_log_user_id",
                table: "sys_user_login_log",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_sys_user_login_log_sys_user_user_id",
                table: "sys_user_login_log",
                column: "user_id",
                principalTable: "sys_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sys_user_login_log_sys_user_user_id",
                table: "sys_user_login_log");

            migrationBuilder.DropIndex(
                name: "IX_sys_user_login_log_user_id",
                table: "sys_user_login_log");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "sys_user_login_log");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace LessSharp.Data.Migrations
{
    public partial class ddcdceab13a7435fa5217a6b77cd59cd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sys_user_login_log_sys_user_create_user_id",
                table: "sys_user_login_log");

            migrationBuilder.DropIndex(
                name: "IX_sys_user_login_log_create_user_id",
                table: "sys_user_login_log");

            migrationBuilder.DropColumn(
                name: "create_user_id",
                table: "sys_user_login_log");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "create_user_id",
                table: "sys_user_login_log",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_sys_user_login_log_create_user_id",
                table: "sys_user_login_log",
                column: "create_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_sys_user_login_log_sys_user_create_user_id",
                table: "sys_user_login_log",
                column: "create_user_id",
                principalTable: "sys_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

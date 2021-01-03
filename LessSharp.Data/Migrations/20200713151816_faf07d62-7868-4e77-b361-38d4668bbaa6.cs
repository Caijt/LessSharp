using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LessSharp.Data.Migrations
{
    public partial class faf07d6278684e77b36138d4668bbaa6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "attach",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    create_time = table.Column<DateTime>(nullable: false),
                    create_user_id = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    size = table.Column<int>(nullable: false),
                    ext = table.Column<string>(nullable: true),
                    path = table.Column<string>(nullable: true),
                    entity_name = table.Column<string>(nullable: true),
                    entity_guid = table.Column<Guid>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    is_public = table.Column<bool>(nullable: false),
                    delete_user_id = table.Column<int>(nullable: true),
                    delete_time = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attach", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_api",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    path = table.Column<string>(nullable: true),
                    is_common = table.Column<bool>(nullable: false),
                    permission_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_api", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_config",
                columns: table => new
                {
                    key = table.Column<string>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_config", x => x.key);
                });

            migrationBuilder.CreateTable(
                name: "sys_menu",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    parent_id = table.Column<int>(nullable: true),
                    path = table.Column<string>(nullable: true),
                    icon = table.Column<string>(nullable: true),
                    parent_ids = table.Column<string>(nullable: true),
                    order = table.Column<int>(nullable: false),
                    is_page = table.Column<bool>(nullable: false),
                    can_multiple_open = table.Column<bool>(nullable: false),
                    has_read = table.Column<bool>(nullable: false),
                    has_write = table.Column<bool>(nullable: false),
                    has_review = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_menu", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_role",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    remarks = table.Column<string>(nullable: true),
                    create_time = table.Column<DateTime>(nullable: false),
                    update_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_menu_api",
                columns: table => new
                {
                    api_id = table.Column<int>(nullable: false),
                    menu_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_menu_api", x => new { x.api_id, x.menu_id });
                    table.ForeignKey(
                        name: "FK_sys_menu_api_sys_api_api_id",
                        column: x => x.api_id,
                        principalTable: "sys_api",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sys_menu_api_sys_menu_menu_id",
                        column: x => x.menu_id,
                        principalTable: "sys_menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sys_role_menu",
                columns: table => new
                {
                    role_id = table.Column<int>(nullable: false),
                    menu_id = table.Column<int>(nullable: false),
                    can_read = table.Column<bool>(nullable: false),
                    can_write = table.Column<bool>(nullable: false),
                    can_review = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_role_menu", x => new { x.role_id, x.menu_id });
                    table.ForeignKey(
                        name: "FK_sys_role_menu_sys_menu_menu_id",
                        column: x => x.menu_id,
                        principalTable: "sys_menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sys_role_menu_sys_role_role_id",
                        column: x => x.role_id,
                        principalTable: "sys_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sys_user",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    login_name = table.Column<string>(nullable: true),
                    login_password = table.Column<string>(nullable: true),
                    role_id = table.Column<int>(nullable: false),
                    is_disabled = table.Column<bool>(nullable: false),
                    create_time = table.Column<DateTime>(nullable: false),
                    update_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_sys_user_sys_role_role_id",
                        column: x => x.role_id,
                        principalTable: "sys_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sys_user_login_log",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ip_address = table.Column<string>(nullable: true),
                    create_time = table.Column<DateTime>(nullable: false),
                    create_user_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_user_login_log", x => x.id);
                    table.ForeignKey(
                        name: "FK_sys_user_login_log_sys_user_create_user_id",
                        column: x => x.create_user_id,
                        principalTable: "sys_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "sys_config",
                columns: new[] { "key", "name", "type", "value" },
                values: new object[,]
                {
                    { "SYSTEM_TITLE", "系统标题", "STRING", "LessAdmin快速开发框架" },
                    { "MENU_BAR_TITLE", "菜单栏标题", "STRING", "LessAdmin" },
                    { "VERSION", "版本号", "STRING", "20200414001" },
                    { "IS_REPAIR", "网站维护", "BOOL", "OFF" },
                    { "LAYOUT", "后台布局", "STRING", "leftRight" },
                    { "LIST_DEFAULT_PAGE_SIZE", "列表默认页容量", "NUMBER", "10" },
                    { "MENU_DEFAULT_ICON", "菜单默认图标", "STRING", "el-icon-menu" },
                    { "SHOW_MENU_ICON", "是否显示菜单图标", "BOOL", "OFF" },
                    { "LOGIN_VCODE", "登录需要验证码", "BOOL", "OFF" },
                    { "PAGE_TABS", "使用多页面标签", "BOOL", "ON" }
                });

            migrationBuilder.InsertData(
                table: "sys_menu",
                columns: new[] { "id", "can_multiple_open", "has_read", "has_review", "has_write", "icon", "is_page", "name", "order", "parent_id", "parent_ids", "path" },
                values: new object[,]
                {
                    { 1, false, false, false, false, null, false, "系统管理", 99, null, null, "sys" },
                    { 2, false, false, false, false, null, false, "用户管理", 1, 1, "1", "user" },
                    { 3, false, false, false, false, null, false, "角色管理", 2, 1, "1", "role" },
                    { 4, false, false, false, false, null, false, "菜单管理", 3, 1, "1", "menu" },
                    { 5, false, false, false, false, null, false, "接口管理", 4, 1, "1", "api" }
                });

            migrationBuilder.InsertData(
                table: "sys_role",
                columns: new[] { "id", "create_time", "name", "remarks", "update_time" },
                values: new object[] { -1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "超级角色", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "sys_user",
                columns: new[] { "id", "create_time", "is_disabled", "login_name", "login_password", "role_id", "update_time" },
                values: new object[] { -1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "superadmin", "admin", -1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_sys_api_name",
                table: "sys_api",
                column: "name",
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_sys_api_path",
                table: "sys_api",
                column: "path",
                unique: true,
                filter: "[path] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_sys_menu_api_menu_id",
                table: "sys_menu_api",
                column: "menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_sys_role_name",
                table: "sys_role",
                column: "name",
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_sys_role_menu_menu_id",
                table: "sys_role_menu",
                column: "menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_sys_user_login_name",
                table: "sys_user",
                column: "login_name",
                unique: true,
                filter: "[login_name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_sys_user_role_id",
                table: "sys_user",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_sys_user_login_log_create_user_id",
                table: "sys_user_login_log",
                column: "create_user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attach");

            migrationBuilder.DropTable(
                name: "sys_config");

            migrationBuilder.DropTable(
                name: "sys_menu_api");

            migrationBuilder.DropTable(
                name: "sys_role_menu");

            migrationBuilder.DropTable(
                name: "sys_user_login_log");

            migrationBuilder.DropTable(
                name: "sys_api");

            migrationBuilder.DropTable(
                name: "sys_menu");

            migrationBuilder.DropTable(
                name: "sys_user");

            migrationBuilder.DropTable(
                name: "sys_role");
        }
    }
}

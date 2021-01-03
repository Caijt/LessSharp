using Microsoft.EntityFrameworkCore.Migrations;

namespace LessSharp.Data.Migrations
{
    public partial class f7f668c871374b4aad5780b76ac4899e : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "sys_api",
                columns: new[] { "id", "is_common", "name", "path", "permission_type" },
                values: new object[,]
                {
                    { 1, false, "获取接口分页列表", "/Sys/Api/GetPageList", 0 },
                    { 2, false, "删除接口", "/Sys/Api/DeleteById", 1 },
                    { 3, false, "保存接口", "/Sys/Api/Save", 1 },
                    { 4, true, "获取接口公共分页列表", "/Sys/Api/GetCommonPageList", 0 },
                    { 5, true, "获取角色公共选项列表", "/Sys/Role/GetCommonOptionList", 0 },
                    { 6, false, "获取角色分页列表", "/Sys/Role/GetPageList", 0 }
                });

            migrationBuilder.UpdateData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "has_read", "has_write" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "has_read", "has_write" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "has_read", "has_write" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "has_read", "has_write" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "has_read", "has_write" },
                values: new object[] { true, true });

            migrationBuilder.UpdateData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "has_read", "has_write" },
                values: new object[] { true, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "sys_api",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "sys_api",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "sys_api",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "sys_api",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "sys_api",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "sys_api",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "has_read", "has_write" },
                values: new object[] { false, false });

            migrationBuilder.UpdateData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "has_read", "has_write" },
                values: new object[] { false, false });

            migrationBuilder.UpdateData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "has_read", "has_write" },
                values: new object[] { false, false });

            migrationBuilder.UpdateData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "has_read", "has_write" },
                values: new object[] { false, false });

            migrationBuilder.UpdateData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "has_read", "has_write" },
                values: new object[] { false, false });

            migrationBuilder.UpdateData(
                table: "sys_menu",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "has_read", "has_write" },
                values: new object[] { false, false });
        }
    }
}

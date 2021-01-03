using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LessSharp.EFHelperConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentPath = System.IO.Directory.GetCurrentDirectory();
            StringBuilder sb = new StringBuilder();

            string dataProjectPath = Path.GetFullPath("..\\..\\..\\..\\LessSharp.Data");
            sb.Append("控制台当前路径为：" + currentPath + "\r\n");
            sb.Append("EF项目绝对路径为：" + dataProjectPath + "\r\n");
            sb.Append("请确定以上路径是否正确，错误会导致以下命令无法正常执行。\r\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(sb);
            Console.ForegroundColor = ConsoleColor.White;
            sb.Clear();
            sb.Append("【EntityFramework Core数据库管理控制台】\r\n");
            sb.Append("1.添加新的迁移\t\t\t【dotnet ef migrations add <fileName>】\r\n");
            sb.Append("2.删除最近的迁移\t\t【dotnet ef migrations remove】\r\n");
            sb.Append("3.查看所有迁移文件\t\t【dotnet ef migrations list】\r\n");
            sb.Append("4.生成迁移Sql\t\t\t【dotnet ef migrations script <from> <to>】\r\n");
            sb.Append("5.更新数据库\t\t\t【dotnet ef database update】\r\n");
            sb.Append("6.删除数据库\t\t\t【dotnet ef database drop】\r\n");
            sb.Append("7.查看数据库详情\t\t【dotnet ef dbcontext info】\r\n");
            sb.Append("8.查看数据库列表\t\t【dotnet ef dbcontext list】\r\n");
            sb.Append("9.反向生成实体及数据库类\t【dotnet ef dbcontext scaffold】\r\n");
            sb.Append("10.安装dotnet-ef工具\t\t【dotnet tool install --global dotnet-ef --version 3.1.5】\r\n");
            sb.Append("11.更新dotnet-ef工具\t\t【dotnet tool update --global dotnet-ef --version 3.1.5】\r\n");
            sb.Append("\r\n0.退出工具\r\n");
            sb.Append("\r\n选择对应的选项，然后回车。");

            while (true)
            {
                Console.WriteLine(sb);
                string input = Console.ReadLine();
                
                if (!int.TryParse(input, out int result))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("输入有误，请重新输入！\r\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
                if (result == 0)
                {
                    break;
                }
                switch (result)
                {
                    case 1:
                        Console.WriteLine("请输入迁移文件名称：（留空自动生成唯一名称）");
                        string fileName = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(fileName))
                        {
                            fileName = Guid.NewGuid().ToString();
                        }
                        CmdHelper.Execute("dotnet ef migrations add " + fileName + " --project " + dataProjectPath);
                        break;
                    case 2:
                        //Console.WriteLine("数据库是否也回滚到迁移前？（输入y进行数据库回滚）");
                        //bool isYes = Console.ReadLine().Equals("y", StringComparison.OrdinalIgnoreCase);
                        //if (isYes)
                        //{
                        //    Console.WriteLine("已启用数据库回滚。");
                        //}
                        CmdHelper.Execute("dotnet ef migrations remove --force --project " + dataProjectPath);
                        break;
                    case 3:
                        CmdHelper.Execute("dotnet ef migrations list --project " + dataProjectPath);
                        break;
                    case 4:
                        Console.WriteLine("请输入要生成迁移Sql的开始迁移ID及结束ID：（留空为全部，0代表第一次迁移前，例如0 Init）");
                        string fromTo = Console.ReadLine();
                        string sqlName = Regex.Replace(fromTo, "\\s+", "_");
                        if (string.IsNullOrEmpty(sqlName))
                        {
                            sqlName = "all";
                        }
                        CmdHelper.Execute("dotnet ef migrations script " + fromTo + " --output " + dataProjectPath + "\\" + sqlName + ".sql --project " + dataProjectPath);
                        break;
                    case 5:
                        Console.WriteLine("请输入要更新的迁移文件名称：（留空为最新）");
                        string updateName = Console.ReadLine();
                        CmdHelper.Execute("dotnet ef database update " + updateName + " --project " + dataProjectPath);
                        break;
                    case 6:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("您确定要删除数据库吗？！确定删除请输入大写的OK，然后回车");
                        Console.ForegroundColor = ConsoleColor.White;
                        string ok = Console.ReadLine();
                        if (ok == "OK")
                        {
                            CmdHelper.Execute("dotnet ef database drop --project " + dataProjectPath + " --force");
                        }
                        else
                        {
                            Console.WriteLine("取消删除。");
                        }

                        break;
                    case 7:
                        CmdHelper.Execute("dotnet ef dbcontext info --project " + dataProjectPath);
                        break;
                    case 8:
                        CmdHelper.Execute("dotnet ef dbcontext list --project" + dataProjectPath);
                        break;
                    case 9:
                        CmdHelper.Execute("dotnet ef dbcontext scaffold --project " + dataProjectPath);
                        break;
                    case 10:
                        CmdHelper.Execute("dotnet tool install --global dotnet-ef --version 3.1.5");
                        break;
                    case 11:
                        CmdHelper.Execute("dotnet tool update --global dotnet-ef --version 3.1.5");
                        break;
                }
                Console.WriteLine("操作已执行，回车继续其它操作。");
                Console.ReadKey();
            }

        }
    }
}

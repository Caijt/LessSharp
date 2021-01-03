using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LessSharp.EFHelperConsole
{
    class CmdHelper
    {
        public static string Execute(string cmd)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("命令：" + cmd);
            cmd = cmd.Trim().TrimEnd('&') + "&exit";
            using (Process p = new Process())
            {
                
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.AutoFlush = true;
                var a = Encoding.Default;
                string result = p.StandardError.ReadToEnd();                
                if (string.IsNullOrEmpty(result))
                {
                    result = ReadFromStream(p.StandardOutput.BaseStream);
                    var lines = result.Split("\r\n").ToList();
                    lines.RemoveRange(0, 4);
                    result = string.Join("\r\n", lines);
                }
                p.WaitForExit();
                p.Close();
                Console.WriteLine("结果：\r\n" + result);
                Console.ForegroundColor = ConsoleColor.White;
                return result;
            }

        }
        private static string ReadFromStream(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (stream)
                {
                    stream.CopyTo(ms);
                }
                return Encoding.UTF8.GetString(ms.GetBuffer());
            }
        }
    }
}

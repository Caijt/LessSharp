using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.Extensions.DependencyInjection;
using LessSharp.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LessSharp.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging((context, loggingBuilder) =>
            {
                var path = context.HostingEnvironment.ContentRootPath;
                loggingBuilder.AddLog4Net($"{path}/log4net.config");
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                var dependAssemblyKey = BuildDependAssemblyKey();
                if (!string.IsNullOrEmpty(dependAssemblyKey))
                {
                    webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, dependAssemblyKey);
                }
            })
            //��AspectCore�滻Ĭ�ϵ�IOC����
            .UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());

        /// <summary>
        /// ������������keyֵ
        /// </summary>
        /// <returns></returns>
        private static string BuildDependAssemblyKey()
        {
            var types = GetDependAssemblyNames(Assembly.GetExecutingAssembly());
            return String.Join(";", types);
        }

        /// <summary>
        /// ��ȡ�������򼯵���������
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static List<string> GetDependAssemblyNames(Assembly assembly)
        {
            var attributes = assembly.GetCustomAttributes<DependOnAttribute>();
            List<string> types = new List<string>();
            foreach (var a in attributes)
            {
                foreach (var type in a.DependTypes)
                {
                    string assemblyName = type.Assembly.GetName().Name;
                    if (types.IndexOf(assemblyName) < 0)
                    {
                        types.Add(assemblyName);
                        types.AddRange(GetDependAssemblyNames(type.Assembly));
                    }

                }
            }
            return types;

        }
    }
}

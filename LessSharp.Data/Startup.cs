using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Storage;

[assembly: HostingStartup(typeof(LessSharp.Data.Startup))]
namespace LessSharp.Data
{
    public class Startup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<AppDbContext>(o =>
                {
                    this.DbContextConfig(o, context.Configuration);
                });
            });
        }

        public Action<DbContextOptionsBuilder, IConfiguration> DbContextConfig = (builder, configuration) =>
         {
             builder.UseMySql(configuration.GetConnectionString("Mysql"), b =>
             {
                 b.ServerVersion(ServerVersion.NullableGeneratedColumnsMySqlSupportVersionString);
                 //b.CharSet(new CharSet("utf8mb4", 250));
                 var charset = CharSet.Utf8Mb4;
                 b.CharSet(new CharSet(charset.Name, 2));

             });
             //builder.UseSqlServer(configuration.GetConnectionString("SqlServer"), b =>
             //{
             //    //2008数据库
             //    //b.UseRowNumberForPaging();
             //});
         };
    }
}

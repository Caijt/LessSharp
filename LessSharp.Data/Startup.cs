using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

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
                    o.UseSqlServer(context.Configuration.GetConnectionString("SqlServer"), b =>
                    {
                        //2008数据库
                        //b.UseRowNumberForPaging();
                    });
                });
            });
        }
    }
}

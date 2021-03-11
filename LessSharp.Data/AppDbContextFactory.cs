using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LessSharp.Data
{
    /// <summary>
    /// 主要是用来配合EF迁移工具
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder();
            var jsonPath = Path.GetFullPath("..\\LessSharp.WebApi\\appsettings.json"); ;
            builder.AddJsonFile(jsonPath);
            var configuration = builder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            //optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            //optionsBuilder.UseMySql(configuration.GetConnectionString("MySql"), b =>
            //{
            //    //b.CharSet(new Pomelo.EntityFrameworkCore.MySql.Storage.CharSet("utf8", 1000));
            // });
            new Startup().DbContextConfig(optionsBuilder, configuration);
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

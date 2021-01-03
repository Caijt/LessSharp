using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using LessSharp.Common;
using LessSharp.Service.Common;

[assembly: HostingStartup(typeof(LessSharp.Service.Startup))]
[assembly: DependOn(typeof(LessSharp.Data.Startup))]
[assembly: DependOn(typeof(LessSharp.ApiService.Startup))]

namespace LessSharp.Service
{
    public class Startup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<AuthContext>();
                services.AddScoped<ConfigContext>();
                foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
                {
                    if (t.IsAbstract || t.IsInterface || !t.IsVisible)
                    {
                        continue;
                    }
                    if (t.Name.EndsWith("Service"))
                    {
                        services.AddTransient(t);
                    }
                    else if (t.IsGenericType)
                    {
                        if (t.GetInterfaces().Any(e => e.GetGenericTypeDefinition() == typeof(IQueryFilter<>)))
                        {
                            services.AddScoped(typeof(IQueryFilter<>), t);
                        }
                        else if (t.GetInterfaces().Any(e => e.GetGenericTypeDefinition() == typeof(IEntityHandler<>)))
                        {
                            services.AddScoped(typeof(IEntityHandler<>), t);
                        }
                    }
                }
                services.AddAutoMapper(typeof(MapperConfiguration.Configuration));
            });
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using LessSharp.ApiService.Qywx;
using Microsoft.Extensions.DependencyInjection;
using LessSharp.ApiService.QQMap;

[assembly: HostingStartup(typeof(LessSharp.ApiService.Startup))]
namespace LessSharp.ApiService
{
    public class Startup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddHttpClient<QywxApiService>();
                services.AddHttpClient<QQMapApiService>();
            });
        }
    }
}

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Vit.Extensions;

namespace App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseServiceAdaptor()
            .UseUrls(Vit.Core.Util.ConfigurationManager.ConfigurationManager.Instance.GetByPath<string[]>("server.urls"))
            .UseStartup<Startup>()
            ;
    }
}

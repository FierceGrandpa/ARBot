using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AR.Bot.Web
{
    public class Program
    {
        public static readonly DateTime StartedTime = DateTime.UtcNow;

        public static async Task Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture     = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture   = CultureInfo.InvariantCulture;
            AppDomain.CurrentDomain.UnhandledException += (_, eventArgs) => Log.Fatal(eventArgs.ExceptionObject.ToString());

            try
            {
                await CreateHostBuilder(args)
                    .Build()
                    .RunAsync();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: {e.Message}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                    .Ignore(new[]
                    {
                        "Microsoft.EntityFrameworkCore.Database.Command",
                        "Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker",
                        "Microsoft.AspNetCore.Hosting.Diagnostics",
                        "Microsoft.AspNetCore.Mvc.StatusCodeResult",
                        "Microsoft.EntityFrameworkCore.Infrastructure"
                    }))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Serilog
using Serilog;

namespace Link.Multicanal.WorkerService
{
    public class Program
    {
        private static IConfiguration Configuration;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    var config = hostContext.Configuration;

                    // Serilog
                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(config)
                        .Enrich.FromLogContext()
                        .CreateLogger();

                    // Logging
                    services.AddLogging(logging =>
                    {
                        logging.AddSerilog();
                    });

                    var appConfig = config.GetSection(Config.SectionKeys.App).Get<Config.AppConfig>();
                    services.AddSingleton(appConfig);

                    services.AddTransient<Link.Multicanal.API.Multicanal.WebService.ServidorSms.AddServicePortTypeClient, Link.Multicanal.API.Multicanal.WebService.ServidorSms.AddServicePortTypeClient>();
                    services.AddTransient<Link.Multicanal.API.Multicanal.WebService.Servidor.AddServicePortTypeClient, Link.Multicanal.API.Multicanal.WebService.Servidor.AddServicePortTypeClient>();
                    services.AddTransient<Link.Multicanal.API.ServidorSmsService, Link.Multicanal.API.ServidorSmsService>();
                    services.AddTransient<Link.Multicanal.API.ServidorService, Link.Multicanal.API.ServidorService>();
                    services.AddTransient<Service.AppService, Service.AppService>();

                    services.AddHostedService<Worker>();

                });
    }
}

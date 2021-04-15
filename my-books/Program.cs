using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books {
    public class Program {
        public static void Main(string[] args) {

            //Define Log - Serilog
            try {
                var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                Log.Logger = new LoggerConfiguration()
                    //Read from json file
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                //Log.Logger = new LoggerConfiguration()
                //    //Store Log to file
                //    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                //    .CreateLogger();
                CreateHostBuilder(args).Build().Run();

            } finally {

                Log.CloseAndFlush();
            }

            }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

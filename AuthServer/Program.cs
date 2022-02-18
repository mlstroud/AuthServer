using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using AuthServer.Library.Config;
using AuthServer.Library.DataAccess;
using Serilog;

namespace AuthServer
{
    public class Program
    {
        private static string _logPath = "Logs/AuthServer_.log";

        public static void Main(string[] args)
        {
            SetupLogging();
            SetupDb();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void SetupLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(_logPath)
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        private static void SetupDb()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
                .Build();

            var dbConfig = config
                .GetSection("DbConfig")
                .Get<DbConfig>();

            if (dbConfig == null)
                throw new Exception($"Unable to locate configuration for {nameof(DbConfig)}");

            DbHelper.SetConfig(dbConfig);
        }
    }
}

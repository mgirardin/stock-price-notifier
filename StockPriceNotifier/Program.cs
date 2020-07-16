using System;
using System.IO;
using CommandDotNet;
using Microsoft.Extensions.Configuration;

namespace StockPriceNotifier
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAppRunner().Run(args);
        }

        public static AppRunner<Commands> BuildAppRunner()
        {
            AppRunner<Commands> appRunner = new AppRunner<Commands>();
            appRunner.UseTypoSuggestions();
            return appRunner;
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}
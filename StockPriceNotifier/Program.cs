using System;
using System.IO;
using CommandDotNet;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StockPriceNotifier
{
    class Program
    {    
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            RegisterServices();

            BuildAppRunner(_serviceProvider).Run(args);

            DisposeServices();
        }

        public static AppRunner<Commands> BuildAppRunner(IServiceProvider serviceProvider)
        {
            AppRunner<Commands> appRunner = new AppRunner<Commands>();
            appRunner.UseMicrosoftDependencyInjection(serviceProvider)
                     .UseTypoSuggestions();
            return appRunner;
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            var config = LoadConfiguration();
            collection.AddSingleton(config);        
            collection.AddSingleton<Commands, Commands>();
            _serviceProvider = collection.BuildServiceProvider();
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        private static void DisposeServices()
        {
            if(_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }

    }
}
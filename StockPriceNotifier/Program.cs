using System;
using System.IO;
using System.Threading;
using CommandDotNet;
using CommandDotNet.IoC.MicrosoftDependencyInjection;
using CommandDotNet.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StockPriceNotifier
{
    public class Program
    {    
        private static ManualResetEvent _quitEvent = new ManualResetEvent(false);
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            RegisterServices();

            BuildAppRunner(_serviceProvider).Run(args);

            Console.CancelKeyPress += delegate {
                DisposeServices();
            };

            _quitEvent.WaitOne();
        }

        public static AppRunner<Commands> BuildAppRunner(IServiceProvider serviceProvider)
        {
            AppRunner<Commands> appRunner = new AppRunner<Commands>();
            appRunner.UseMicrosoftDependencyInjection(serviceProvider)
                     .UseDataAnnotationValidations(showHelpOnError: true)
                     .UseTypoSuggestions();
            return appRunner;
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            var config = LoadConfiguration();
            collection.AddSingleton(config);        
            collection.AddSingleton<Commands, Commands>();
            collection.AddSingleton<IStockPriceScheduler, StockPriceScheduler>();
            collection.AddHttpClient();
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
using System;
using CommandDotNet;

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
    }
}
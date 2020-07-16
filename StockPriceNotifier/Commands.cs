using System;
using CommandDotNet;
using CommandDotNet.Rendering;
using System.ComponentModel.DataAnnotations;

namespace StockPriceNotifier
{
    public class Commands
    {
        [DefaultMethod]
        [Command(Name="subscribe", Description = "Create threshold values for specific stock")]
        public async void Subscribe(IConsole console,            
            [Operand(Description = "Name of the stock you want to subscribe")]
            [Required(ErrorMessage = "Stock name is required.")] 
            string stockName, 
            [Operand(Description = "Threshold to sell stock")]
            [Required(ErrorMessage = "Maximum stock price is required.")] 
            float maxPrice, 
            [Operand(Description = "Threshold to buy stock")]
            [Required(ErrorMessage = "Minimum stock price is required.")]  
            float minPrice)
        {
            console.WriteLine(String.Format("Stock {0} job added succesfully.", stockName));
        }
    }
}
using System.Collections.Generic;

namespace StockPriceNotifier
{
    public class StockSearch{
        public bool valid_key { get; set; }
        public Dictionary<string, Stock> results { get; set; }
    }
}
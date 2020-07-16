using System;
using System.Threading.Tasks;

namespace StockPriceNotifier
{
    public interface IStockPriceScheduler {
        public Task Start();

        public Task ScheduleStockPriceJob(String stockName, float maxPrice, float minPrice);
    }
}
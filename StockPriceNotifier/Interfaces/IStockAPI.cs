using System.Threading.Tasks;

namespace StockPriceNotifier
{
    public interface IStockAPI {    
        public Task<float> getStockPrice(string stockName);
    }
}
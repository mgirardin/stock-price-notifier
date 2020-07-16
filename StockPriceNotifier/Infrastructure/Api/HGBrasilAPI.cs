using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockPriceNotifier
{
    public class HGBrasilAPI : IStockAPI {    
        private static IHttpClientFactory _clientFactory;        
        private static readonly String API_URL = "https://api.hgbrasil.com/finance/";
        private static String API_KEY = "5cb82ebf"; // TODO: Should be at least an env variable 
        public HGBrasilAPI(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<float> getStockPrice(string stockName){
            var client = _clientFactory.CreateClient();
            var streamTask = client.GetStreamAsync(String.Format("{0}/stock_price?key={1}&symbol={2}", 
                                                                API_URL, API_KEY, stockName));
            var stockSearch = await JsonSerializer.DeserializeAsync<StockSearch>(await streamTask);
            Stock stock = stockSearch.results[stockName];
            return stock.price;
        }
    }
}
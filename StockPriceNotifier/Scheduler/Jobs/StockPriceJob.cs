using System;
using Quartz;
using System.Threading.Tasks;

namespace StockPriceNotifier
{
    [PersistJobDataAfterExecution]
    public class StockPriceJob : IJob {

        IMailService _mailService;
        IStockAPI _api;

        public StockPriceJob(IMailService mailService, IStockAPI api){
            _mailService = mailService;
            _api = api;
        }
        
        private async Task<string> handleStockPriceNotification(string stockName, string maxPrice, string minPrice, string status){
            try{
                var stockPrice = await _api.getStockPrice(stockName);
                if(stockPrice > float.Parse(maxPrice) && status != "SELL"){
                    status = "SELL";
                    _mailService.SendEmail("", "Venda - Stock " + stockName); // TODO: Use template for email
                }
                else if(stockPrice < float.Parse(minPrice) && status != "BUY"){
                    status = "BUY";
                    _mailService.SendEmail("", "Compra - Stock " + stockName); // TODO: Use template for email
                }
            }
            catch(Exception e){
                Console.WriteLine(e);
                Console.WriteLine("API Key no longer valid.");
            }
            return status;
        }

        public async Task Execute(IJobExecutionContext context){
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string stockName = dataMap.GetString("stockName");
            string maxPrice = dataMap.GetString("maxPrice");
            string minPrice = dataMap.GetString("minPrice");
            string status = dataMap.GetString("status");
            dataMap["status"] = await handleStockPriceNotification(stockName, maxPrice, minPrice, status);
        }
    }
}
using System;
using Quartz;
using System.Threading.Tasks;

namespace StockPriceNotifier
{
    [PersistJobDataAfterExecution]
    public class StockPriceJob : IJob {
        private string handleStockPriceNotification(string stockName, string maxPrice, string minPrice, string status){
            try{
                var stockPrice = 15;
                if(stockPrice > float.Parse(maxPrice) && status != "SELL"){
                    status = "SELL";
                }
                else if(stockPrice < float.Parse(minPrice) && status != "BUY"){
                    status = "BUY";
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

            dataMap["status"] = handleStockPriceNotification(stockName, maxPrice, minPrice, status);
        }
    }
}
using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace StockPriceNotifier
{
    public class StockPriceScheduler : IStockPriceScheduler {

        IScheduler _scheduler = null;

        public async Task Start(){
            try{
                if(_scheduler != null){
                    return;
                }
                StdSchedulerFactory factory = new StdSchedulerFactory();
                _scheduler = await factory.GetScheduler();
                await _scheduler.Start();
            }
            catch (SchedulerException se)
            {
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }

        public async Task ScheduleStockPriceJob(String stockName, float maxPrice, float minPrice){
            IJobDetail job = JobBuilder.Create<StockPriceJob>()
            	.UsingJobData("stockName", stockName)
                .UsingJobData("maxPrice", maxPrice.ToString())
                .UsingJobData("minPrice", minPrice.ToString())
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(60)
                    .RepeatForever())
                .Build();
            await _scheduler.ScheduleJob(job, trigger);
        }
    }
}
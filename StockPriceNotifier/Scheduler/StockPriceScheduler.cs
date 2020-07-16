using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Microsoft.Extensions.Configuration;


namespace StockPriceNotifier
{
    public class StockPriceScheduler : IStockPriceScheduler {

        private IScheduler _scheduler = null;
        private IJobFactory _jobFactory = null;
        private int _polling_interval;

        public StockPriceScheduler(IJobFactory jobFactory, IConfiguration config){
            _jobFactory = jobFactory;
            _polling_interval = config.GetValue<int>("PollingInterval");
        }

        public async Task Start(){
            try{
                if(_scheduler != null){
                    return;
                }
                StdSchedulerFactory factory = new StdSchedulerFactory();
                _scheduler = await factory.GetScheduler();
                _scheduler.JobFactory = _jobFactory;
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
                    .WithIntervalInSeconds(_polling_interval)
                    .RepeatForever())
                .Build();
            await _scheduler.ScheduleJob(job, trigger);
        }
    }
}
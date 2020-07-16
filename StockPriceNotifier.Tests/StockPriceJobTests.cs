using System;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using Moq;
using Xunit;
using StockPriceNotifier;

namespace UnitTests
{
    public class StockPriceJobTest
    {
        [Theory]
        [InlineData("PETR4", "2", "1", "")] // SELL STOCK        
        [InlineData("PETR4", "2", "1", "SELL")] // ALREADY NOTIFIED
        public async void shouldIdentifyTimeToSell(String stockName, string maxPrice, string minPrice, string status){            
            var mockStockAPI = new Mock<IStockAPI>();  
            var mockMailService = new Mock<IMailService>();
            mockStockAPI.Setup(p => p.getStockPrice(It.IsAny<string>())).Returns(Task.FromResult(15.00f));
            
            StockPriceJob stockPriceJob = new StockPriceJob(mockMailService.Object, mockStockAPI.Object);
            Type type = typeof(StockPriceJob);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name == "handleStockPriceNotification" && x.IsPrivate)
                .First();
            Task<string> statusTask = (Task<string>) method.Invoke(stockPriceJob, new object [] {stockName, maxPrice, minPrice, status});
            string new_status = await statusTask;
            Assert.Equal("SELL", new_status);
        }
        
        [Theory]
        [InlineData("PETR4", "30", "20", "")] // BUY STOCK
        [InlineData("PETR4", "30", "20", "BUY")] // ALREADY NOTIFIED
        public async void shouldIdentifyTimeToBuy(String stockName, string maxPrice, string minPrice, string status){            
            var mockStockAPI = new Mock<IStockAPI>();  
            var mockMailService = new Mock<IMailService>();
            mockStockAPI.Setup(p => p.getStockPrice(It.IsAny<string>())).Returns(Task.FromResult(15.00f));

            StockPriceJob stockPriceJob = new StockPriceJob(mockMailService.Object, mockStockAPI.Object);
            Type type = typeof(StockPriceJob);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name == "handleStockPriceNotification" && x.IsPrivate)
                .First();
            Task<string> statusTask = (Task<string>) method.Invoke(stockPriceJob, new object [] {stockName, maxPrice, minPrice, status});
            string new_status = await statusTask;
            Assert.Equal("BUY", new_status);
        }

        [Theory]
        [InlineData("PETR4", "2", "1", "")] // SELL STOCK        
        [InlineData("PETR4", "30", "20", "")] // BUY STOCK
        public async void shouldInvokeEmail(String stockName, string maxPrice, string minPrice, string status){            
            var mockStockAPI = new Mock<IStockAPI>();  
            var mockMailService = new Mock<IMailService>();
            mockStockAPI.Setup(p => p.getStockPrice(It.IsAny<string>())).Returns(Task.FromResult(15.00f));

            StockPriceJob stockPriceJob = new StockPriceJob(mockMailService.Object, mockStockAPI.Object);
            Type type = typeof(StockPriceJob);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name == "handleStockPriceNotification" && x.IsPrivate)
                .First();
            Task<string> statusTask = (Task<string>) method.Invoke(stockPriceJob, new object [] {stockName, maxPrice, minPrice, status});
            mockMailService.Verify(mock => mock.SendEmail(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Theory]
        [InlineData("PETR4", "30", "20", "BUY")] // ALREADY NOTIFIED
        [InlineData("PETR4", "2", "1", "SELL")] // ALREADY NOTIFIED
        [InlineData("PETR4", "20", "10", "BUY")] // Not over threshold
        [InlineData("PETR4", "20", "10", "SELL")] // Not over threshold
        [InlineData("PETR4", "20", "10", "")] // Not over threshold
        public async void shouldntInvokeEmail(String stockName, string maxPrice, string minPrice, string status){            
            var mockStockAPI = new Mock<IStockAPI>();  
            var mockMailService = new Mock<IMailService>();
            mockStockAPI.Setup(p => p.getStockPrice(It.IsAny<string>())).Returns(Task.FromResult(15.00f));

            StockPriceJob stockPriceJob = new StockPriceJob(mockMailService.Object, mockStockAPI.Object);
            Type type = typeof(StockPriceJob);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name == "handleStockPriceNotification" && x.IsPrivate)
                .First();
            Task<string> statusTask = (Task<string>) method.Invoke(stockPriceJob, new object [] {stockName, maxPrice, minPrice, status});
            mockMailService.Verify(mock => mock.SendEmail(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }
    }
}
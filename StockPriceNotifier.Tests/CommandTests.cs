using StockPriceNotifier;
using Xunit;
using CommandDotNet;
using CommandDotNet.TestTools;
using System;
using System.Threading.Tasks;
using Moq;  

namespace UnitTests
{
    public class CommandsTest {

        [Theory]
        [InlineData("", "", "")]
        [InlineData("PETR4", "", "")]
        [InlineData("PETR4", "2", "")]
        public async void subscribeShouldntAcceptEmptyValues(string stockName, string maxPrice, string minPrice){
            var mockScheduler = new Mock<IStockPriceScheduler>();  
            mockScheduler.Setup(p => p.Start()).Returns(Task.CompletedTask);
            mockScheduler.Setup(p => p.ScheduleStockPriceJob(It.IsAny<string>(), 
                                                             It.IsAny<float>(),
                                                             It.IsAny<float>()))
                                                             .Returns(Task.CompletedTask);
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IStockPriceScheduler)))
                .Returns(mockScheduler.Object);
            Commands commandsObject = new Commands(mockScheduler.Object);
            serviceProvider
                .Setup(x => x.GetService(typeof(Commands)))
                .Returns(commandsObject);
            
            var result = Program.BuildAppRunner(serviceProvider.Object)
                                .AppendPipedInputToOperandList()
                                .RunInMem(String.Format("{0} {1} {2}", stockName, maxPrice, minPrice));
            
            int errorCode = await ExitCodes.ValidationError;
            Assert.Equal(errorCode, result.ExitCode);
        }

        [Theory]
        [InlineData("PETR4", "2", "1")]
        public void subscribeShouldExecuteSuccessfully(string stockName, string maxPrice, string minPrice){
            var mockScheduler = new Mock<IStockPriceScheduler>();  
            mockScheduler.Setup(p => p.Start()).Returns(Task.CompletedTask);
            mockScheduler.Setup(p => p.ScheduleStockPriceJob(It.IsAny<string>(), 
                                                             It.IsAny<float>(),
                                                             It.IsAny<float>()))
                                                             .Returns(Task.CompletedTask);
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IStockPriceScheduler)))
                .Returns(mockScheduler.Object);
            Commands commandsObject = new Commands(mockScheduler.Object);
            serviceProvider
                .Setup(x => x.GetService(typeof(Commands)))
                .Returns(commandsObject);
            
            var result = Program.BuildAppRunner(serviceProvider.Object)
                                .AppendPipedInputToOperandList()
                                .RunInMem(String.Format("{0} {1} {2}", stockName, maxPrice, minPrice));
            Assert.Equal(0, result.ExitCode);
        }
    }
    
}
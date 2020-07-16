using StockPriceNotifier;
using Xunit;
using CommandDotNet;
using CommandDotNet.TestTools;
using System;
using Moq;  

namespace UnitTests
{
    public class CommandsTest {

        [Theory]
        [InlineData("", "", "")]
        [InlineData("PETR4", "", "")]
        [InlineData("PETR4", "2", "")]
        public async void subscribeShouldntAcceptEmptyValues(string stockName, string maxPrice, string minPrice){
            var serviceProvider = new Mock<IServiceProvider>();
            Commands commandsObject = new Commands();
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
        public async void subscribeShouldExecuteSuccessfully(string stockName, string maxPrice, string minPrice){
            var serviceProvider = new Mock<IServiceProvider>();
            Commands commandsObject = new Commands();
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
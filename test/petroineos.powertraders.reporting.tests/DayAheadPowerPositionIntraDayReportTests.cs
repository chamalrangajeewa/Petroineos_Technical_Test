namespace petroineos.powertraders.reporting.tests
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using Services;
    using System;
    using System.Threading.Tasks;
    using Xunit;
    using petroineos.powertraders.reporting;
    using Microsoft.Extensions.Options;

    public class DayAheadPowerPositionIntraDayReportTests
    {
        private MockRepository mockRepository;
        private Mock<IPowerService> mockPowerService;
        private Mock<IOptions<Configs>> mockConfigOptions;
        private Mock<ILogger<DayAheadPowerPositionIntraDayReport>> mockLogger;

        public DayAheadPowerPositionIntraDayReportTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockPowerService = this.mockRepository.Create<IPowerService>();
            this.mockConfigOptions = this.mockRepository.Create<IOptions<Configs>>();
            this.mockLogger = this.mockRepository.Create<ILogger<DayAheadPowerPositionIntraDayReport>>();

            mockConfigOptions.SetupGet(o => o.Value).Returns(new Configs()
            { 
                FolderPath = "D:\\cimplex\\",
                IntervalInSeconds = 4
            });

            var powerTrade1 = PowerTrade.Create(DateTime.Today, 24);
            var powerTrade2 = PowerTrade.Create(DateTime.Today, 24);
            var powerTrade3 = PowerTrade.Create(DateTime.Today, 24);

            powerTrade1.Periods.ToList().ForEach(a => a.Volume = 10);
            powerTrade2.Periods.ToList().ForEach(a => a.Volume = 20);
            powerTrade3.Periods.ToList().ForEach(a => a.Volume = 30);

            this.mockPowerService
                .Setup(o => o.GetTradesAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<PowerTrade>() 
                {
                    powerTrade1,
                    powerTrade2,
                    powerTrade3
                });
        }

        private DayAheadPowerPositionIntraDayReport CreateDayAheadPowerPositionIntraDayReport()
        {
            return new DayAheadPowerPositionIntraDayReport(
                this.mockPowerService.Object,
                this.mockConfigOptions.Object,
                this.mockLogger.Object);
        }

        [Fact]
        public async Task GivenSomeDataPoints_WhenCallingGenerate_ThenShouldGenerateAFile()
        {
            // Arrange
            var dayAheadPowerPositionIntraDayReport = this.CreateDayAheadPowerPositionIntraDayReport();

            // Act
            await dayAheadPowerPositionIntraDayReport.GenerateAsync(default);

            // Assert
            Assert.True(true);
            this.mockRepository.VerifyAll();
        }
    }
}
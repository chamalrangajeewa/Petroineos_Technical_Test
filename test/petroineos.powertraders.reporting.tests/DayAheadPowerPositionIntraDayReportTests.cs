namespace petroineos.powertraders.reporting.tests
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using Services;
    using System;
    using System.Threading.Tasks;
    using Xunit;
    using petroineos.powertraders.reporting;

    public class DayAheadPowerPositionIntraDayReportTests
    {
        private MockRepository mockRepository;
        private Mock<IPowerService> mockPowerService;
        private Mock<ILogger<DayAheadPowerPositionIntraDayReport>> mockLogger;

        public DayAheadPowerPositionIntraDayReportTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockPowerService = this.mockRepository.Create<IPowerService>();
            this.mockLogger = this.mockRepository.Create<ILogger<DayAheadPowerPositionIntraDayReport>>();

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
                this.mockLogger.Object);
        }

        [Fact]
        public async Task Generate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dayAheadPowerPositionIntraDayReport = this.CreateDayAheadPowerPositionIntraDayReport();

            // Act
            await dayAheadPowerPositionIntraDayReport.GenerateAsync(default);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
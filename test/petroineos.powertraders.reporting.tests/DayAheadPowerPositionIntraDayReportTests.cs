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
            await dayAheadPowerPositionIntraDayReport.Generate();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}

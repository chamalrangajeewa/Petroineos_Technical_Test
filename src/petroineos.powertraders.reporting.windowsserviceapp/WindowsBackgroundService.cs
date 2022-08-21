using Microsoft.Extensions.Options;

namespace petroineos.powertraders.reporting.windowsserviceapp
{
    public sealed class WindowsBackgroundService : BackgroundService
    {
        private readonly IReport _report;
        private readonly ILogger<WindowsBackgroundService> _logger;
        private readonly IOptions<Configs> _configs;

        public WindowsBackgroundService(
            IReport report,
            IOptions<Configs> configs,
            ILogger<WindowsBackgroundService> logger)
        {
            _report = report;
            _logger = logger;
            _configs = configs;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await _report.GenerateAsync(stoppingToken);
                    await Task.Delay(TimeSpan.FromSeconds(_configs.Value.IntervalInSeconds), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
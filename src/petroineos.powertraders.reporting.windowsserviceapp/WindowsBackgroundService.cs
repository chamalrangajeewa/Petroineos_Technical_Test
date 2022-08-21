namespace petroineos.powertraders.reporting.windowsserviceapp
{
    using Microsoft.Extensions.Options;

    public sealed class WindowsBackgroundService : BackgroundService
    {
        private readonly IReport _report;
        private readonly ILogger<WindowsBackgroundService> _logger;
        private readonly IOptions<Configs> _configs;

        private int errorCounter = 0;
        private const int errrorToleranceCounter = 20;

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
            _logger.LogInformation("Windows service background worker started running at: {time}", DateTimeOffset.Now);

            try
            {
                bool folderExist = Directory.Exists(_configs.Value.FolderPath);
                if (!folderExist) Directory.CreateDirectory(_configs.Value.FolderPath);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        await _report.GenerateAsync(stoppingToken);
                        errorCounter = 0;
                    }
                    catch (Exception ex)
                    {
                        if (errorCounter >= errrorToleranceCounter)
                        {
                            throw;
                        }
                        _logger.LogError(ex, "{Message}", ex.Message);
                        errorCounter++;                       
                    }

                    _logger.LogInformation("waiting for next run.next run will be in {0}", _configs.Value.IntervalInSeconds); ;
                    await Task.Delay(TimeSpan.FromSeconds(_configs.Value.IntervalInSeconds), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                _logger.LogCritical("service unstable and failed continuously.please check log to understand the reason.");
                Environment.Exit(1);
            }
        }
    }
}
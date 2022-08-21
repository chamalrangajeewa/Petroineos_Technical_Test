namespace petroineos.powertraders.reporting
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IReport _report;
        private readonly IHost _host;
        private readonly IOptions<Configs> _configs;

        public Worker(
            IReport report,
            IHost host,
            IOptions<Configs> configs,
            ILogger<Worker> logger)
        {
            _logger = logger;
            _report = report;
            _host = host;
            _configs = configs;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

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

            try
            {
                await _report.GenerateAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(_configs.Value.IntervalInSeconds), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                Environment.Exit(1);
            }            

            _logger.LogInformation("Worker finishing at: {time}", DateTimeOffset.Now);

            _host.StopAsync();
        }
    }
}
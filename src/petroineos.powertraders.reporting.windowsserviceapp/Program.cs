namespace petroineos.powertraders.reporting.windowsserviceapp
{
    using Microsoft.Extensions.Hosting;
    using Services;

    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host
                .CreateDefaultBuilder(args)
                .UseWindowsService(options =>
                {
                    options.ServiceName = "Intra-day reporting service";
                })
                .ConfigureServices((ctx,services) =>
                {
                    services.AddHostedService<WindowsBackgroundService>();
                    services.AddOptions<Configs>()
                    .Configure((a) =>
                    {
                        var config = ctx.Configuration;
                        string targetFilePath = config.GetRequiredSection("TargetFilePath").Get<string>();
                        int intervalInSeconds = config.GetRequiredSection("IntervalInSeconds").Get<int>();
                        a.FolderPath = targetFilePath;
                        a.IntervalInSeconds = intervalInSeconds;
                    });

                    services.AddLogging();                   
                    services.AddTransient<IPowerService, PowerService>();
                    services.AddTransient<IReport, DayAheadPowerPositionIntraDayReport>();
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddConfiguration(
                        context.Configuration.GetSection("Logging"));
                })
                .Build();

            host.Run();
        }
    }
}
namespace petroineos.powertraders.reporting.windowsserviceapp
{
    using Microsoft.Extensions.Hosting;

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
                .ConfigureServices(services =>
                {
                    services.AddHostedService<WindowsBackgroundService>();
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
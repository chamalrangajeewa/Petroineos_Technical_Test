namespace petroineos.powertraders.reporting
{
    using CommandLine;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services;

    internal partial class Program
    {
        static void Main(string[] args)
        {            
            var result = Parser.Default.ParseArguments<Options>(args);

            result.WithParsedAsync(o =>
            {
                Console.WriteLine("Building Host....");
                IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                { 
                    services.AddOptions<Configs>().Configure((a) =>
                    {
                        a.FolderPath = result.Value.FolderPath;
                        a.IntervalInSeconds = result.Value.IntervalInSeconds;
                    });

                    services.AddLogging();
                    services.AddHostedService<Worker>();

                    services.AddTransient<IPowerService, PowerService>();
                    services.AddTransient<IReport, DayAheadPowerPositionIntraDayReport>();

                })
                .Build();

                Console.WriteLine("Host building Complete....");
                Console.WriteLine("Preparing to start host....");
                host.Run();

                return Task.CompletedTask;
            });
        }
    }
}
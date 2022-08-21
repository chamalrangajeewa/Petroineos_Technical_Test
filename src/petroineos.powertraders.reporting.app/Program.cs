namespace petroineos.powertraders.reporting
{
    //using Azure;
    //using Azure.Storage.Blobs;
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
                    services.AddHttpClient<Worker>(
                    client =>
                    {
                        client.Timeout = new TimeSpan(0, 20, 0);
                    });

                    //services.AddOptions<CatalogueFeedData>().Configure((a) =>
                    //{
                    //    a.FeedUrl = result.Value.FeedUrl;
                    //    a.TargetFileName = result.Value.TargetFileName;
                    //});

                    services.AddLogging();
                    services.AddHostedService<Worker>();


                    services.AddTransient<IPowerService, PowerService>();
                    //services.AddTransient<BlobContainerClient>(o =>
                    //{
                    //    var config = o.GetRequiredService<IConfiguration>();
                    //    string storageAccountName = config.GetRequiredSection("StorageAccount:Name").Get<string>();
                    //    string blobContainerUri = config.GetRequiredSection("StorageAccount:BlobContainerUri").Get<string>();
                    //    string sasToken = config.GetRequiredSection("StorageAccount:SasToken").Get<string>();

                    //    return new BlobContainerClient
                    //    (
                    //        new Uri(blobContainerUri),
                    //        new AzureSasCredential(sasToken)
                    //    );
                    //});
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
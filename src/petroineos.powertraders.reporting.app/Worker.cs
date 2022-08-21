namespace petroineos.powertraders.reporting
{
    //using Azure.Storage.Blobs;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Services;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPowerService _powerService;
        //private readonly BlobContainerClient _blobContainerClient;
        private readonly IHost _host;
        //private readonly IOptions<CatalogueFeedData> _catalogueFeedData;

        public Worker(
            IPowerService powerService,
            //BlobContainerClient blobContainerClient,
            IHost host,
            //IOptions<CatalogueFeedData> catalogueFeedData,
            ILogger<Worker> logger)
        {
            _logger = logger;
            _powerService = powerService;
           // _blobContainerClient = blobContainerClient;
            _host = host;
           // _catalogueFeedData = catalogueFeedData;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            
            //string fileName = _catalogueFeedData.Value.TargetFileName ?? $"{DateTime.Today.ToString("MM_dd_yyyy")}-{Guid.NewGuid()}.csv";

            //_logger.LogInformation("Downloading catalogue feed from url : {0}", _catalogueFeedData.Value.FeedUrl);
            //var bytes = await _client.GetByteArrayAsync(_catalogueFeedData.Value.FeedUrl, stoppingToken);

            //using (var originalInputFeed = new MemoryStream(bytes))
            //{
            //    using (var transformedOutputFeed = _catalogueFeedService.Tranform(originalInputFeed))
            //    {
            //        transformedOutputFeed.Seek(0, SeekOrigin.Begin);

            //        _logger.LogInformation("Transform Complete. Uploading file to storage");

            //        BlobClient blob = _blobContainerClient.GetBlobClient($"/feeds/{fileName}");
            //        await blob.UploadAsync(transformedOutputFeed, true, stoppingToken);
            //        _logger.LogInformation($"Uploaded File:{blob.Name} to container:{blob.BlobContainerName} at storageAccount:{blob.AccountName}");
            //    }
            //}

            _logger.LogInformation("Worker finishing at: {time}", DateTimeOffset.Now);

            _host.StopAsync();

            _logger.LogInformation("Host stop");
        }
    }
}
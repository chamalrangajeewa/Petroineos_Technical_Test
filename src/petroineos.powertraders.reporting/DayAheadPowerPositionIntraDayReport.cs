namespace petroineos.powertraders.reporting
{
    using CsvHelper;
    using CsvHelper.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Services;
    using System.Globalization;

    public class DayAheadPowerPositionIntraDayReport : IReport
    {
        private readonly static CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            LeaveOpen = true,
            NewLine = Environment.NewLine,
        };

        private readonly static Dictionary<int, string> hourMapping = new Dictionary<int, string>()
        {
            {1,"23:00"},
            {2,"00:00"},
            {3,"01:00"},
            {4,"02:00"},
            {5,"03:00"},
            {6,"04:00"},
            {7,"05:00"},
            {8,"06:00"},
            {9,"07:00"},
            {10,"08:00"},
            {11,"09:00"},
            {12,"10:00"},
            {13,"11:00"},
            {14,"12:00"},
            {15,"13:00"},
            {16,"14:00"},
            {17,"15:00"},
            {18,"16:00"},
            {19,"17:00"},
            {20,"18:00"},
            {21,"19:00"},
            {22,"20:00"},
            {23,"21:00"},
            {24,"22:00"},
        };

        private readonly ILogger<DayAheadPowerPositionIntraDayReport> _logger;
        private readonly IPowerService _powerService;
        private readonly IOptions<Configs> _configs;

        public DayAheadPowerPositionIntraDayReport(
            IPowerService powerService,
            IOptions<Configs> configs,
            ILogger<DayAheadPowerPositionIntraDayReport> logger)
        {
            _logger = logger;
            _powerService = powerService;
            _configs = configs;
        }

        public async Task GenerateAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("started generating report");
            var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            DateTime londonLocalTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, ukTimeZone);

            var powerTrades = await _powerService.GetTradesAsync(londonLocalTime.Date); //This method should have cancellation Token input

            var aggregatedPowerPeriods = from powerTrade in powerTrades
                                         from period in powerTrade.Periods
                                         group period by period.Period into g
                                         select new PowerPeriod()
                                         {
                                             Period = g.Key,
                                             Volume = g.Sum(p => p.Volume),
                                         };

            string fileName = $"PowerPosition_{londonLocalTime.Date:yyyyMMdd}_{londonLocalTime:HHMM}.csv";
            string fullyQualifiedFilePath = Path.Combine(_configs.Value.FolderPath, fileName);

            if (File.Exists(fullyQualifiedFilePath))
            {
                _logger.LogWarning("report file {0} already exist. skipping..", fullyQualifiedFilePath);
            }

            WriteRecordsToCsv(aggregatedPowerPeriods, fullyQualifiedFilePath);
            _logger.LogInformation("Generating report complete");
            _logger.LogInformation("report generated at {0}", fullyQualifiedFilePath);
        }

        private void WriteRecordsToCsv(IEnumerable<PowerPeriod> records, string fileNameWithFolderPath)
        {
            _logger.LogInformation("writting {0} records to file", records.Count());
            using (var writer = new StreamWriter(fileNameWithFolderPath))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.Context.RegisterClassMap<LineEntryMap>();
                csv.WriteHeader<LineEntry>();
                csv.NextRecord();
                foreach (var record in records)
                {
                    _logger.LogInformation("writting power period entry period:{0} volume:{1}", record.Period, record.Volume);
                    csv.WriteRecord(new LineEntry() 
                    {
                        Volume = record.Volume.ToString(),
                        LocalTime = hourMapping[record.Period]
                    });

                    csv.NextRecord();
                }
            }
        }
    }
}
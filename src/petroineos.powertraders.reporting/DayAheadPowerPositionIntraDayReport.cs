namespace petroineos.powertraders.reporting
{
    using CsvHelper;
    using CsvHelper.Configuration;
    using Microsoft.Extensions.Logging;
    using Services;
    using System.Collections.ObjectModel;
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

        public DayAheadPowerPositionIntraDayReport(
            IPowerService powerService,

            ILogger<DayAheadPowerPositionIntraDayReport> logger)
        {
            _logger = logger;
            _powerService = powerService;
        }

        public async Task GenerateAsync(CancellationToken stoppingToken)
        {
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
            string folderPath = "D:\\cimplex\\";
            string fullyQualifiedFilePath = Path.Combine(folderPath, fileName);

            WriteRecordsToCsv(aggregatedPowerPeriods, fullyQualifiedFilePath);
        }

        private void WriteRecordsToCsv(IEnumerable<PowerPeriod> records, string fileNameWithFolderPath)
        {             
            using (var writer = new StreamWriter(fileNameWithFolderPath))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.Context.RegisterClassMap<LineEntryMap>();
                csv.WriteHeader<LineEntry>();
                csv.NextRecord();
                foreach (var record in records)
                {
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
namespace petroineos.powertraders.reporting
{
    using CsvHelper;
    using Microsoft.Extensions.Logging;
    using System.Collections.ObjectModel;
    using System.Globalization;

    public class DayAheadPowerPositionIntraDayReport
    {
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

        public DayAheadPowerPositionIntraDayReport(
            ILogger<DayAheadPowerPositionIntraDayReport> logger)
        {
            _logger = logger;
        }

        public async Task Generate() 
        {
            string fileName = $"PowerPosition_YYYYMMDD_HHMM.csv";
            await Task.CompletedTask;
        }

        private Stream WriteRecordsToCsv(IEnumerable<LineEntry> records)
        {
            MemoryStream resultToReturn = new MemoryStream();

            using (var writer = new StreamWriter(resultToReturn, leaveOpen: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture, true))
            {
                csv.Context.RegisterClassMap<LineEntryMap>();
                csv.WriteHeader<LineEntry>();
                csv.NextRecord();
                foreach (var record in records)
                {
                    csv.WriteRecord(record);
                    csv.NextRecord();
                }
            }

            return resultToReturn;
        }
    }
}
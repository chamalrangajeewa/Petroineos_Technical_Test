namespace petroineos.powertraders.reporting
{
    using CsvHelper.Configuration;

    public class LineEntryMap : ClassMap<LineEntry>
    {
        public LineEntryMap()
        {
            Map(m => m.LocalTime).Index(0).Name("Local Time");
            Map(m => m.Volume).Index(1).Name("Volume");
        }
    }
}

namespace petroineos.powertraders.reporting
{
    using CommandLine;
    using CommandLine.Text;

    internal partial class Program
    {
        public class Options
        {
            [Option(shortName: 'f', longName: "filename", Required = true, HelpText = "The filename to use when uploading the transformed result")]
            public string TargetFileName { get; set; }

            [Option(shortName: 'u', longName: "url", Required = true, HelpText = "The url of the input catalogue feed")]
            public Uri FeedUrl { get; set; }

            [Usage(ApplicationAlias = "preezie.catalog.importer.app.exe")]
            public static IEnumerable<Example> Examples
            {
                get
                {
                    return new List<Example>()
                    {
                        new Example(
                            "Tranform the puma feed given on the url to an one that is compatible with preezie data import ( REMEMBER TO ESCAPE SPECIAL CHARACTERS )",
                            new Options
                            {
                                TargetFileName = "puma_products.csv",
                                FeedUrl = new Uri("https://au.puma.com/pub/media/feeds/preezie_feed.xml")
                            })
                    };
                }
            }
        }
    }
}
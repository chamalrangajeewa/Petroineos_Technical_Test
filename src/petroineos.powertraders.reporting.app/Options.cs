namespace petroineos.powertraders.reporting
{
    using CommandLine;
    using CommandLine.Text;

    internal partial class Program
    {
        public class Options
        {
            [Option(shortName: 'f', longName: "folderpath", Required = true, HelpText = "The folderpath where file generated will be saved")]
            public string FolderPath { get; set; }

            [Option(shortName: 'i', longName: "interval", Required = true, HelpText = "The schedule Interval")]
            public int IntervalInSeconds { get; set; }

            [Usage(ApplicationAlias = "petroineos.powertraders.reporting.app.exe")]
            public static IEnumerable<Example> Examples
            {
                get
                {
                    return new List<Example>()
                    {
                        new Example(
                            "Generate intra day report for day ahead power position. The reported is generate with the interval given.",
                            new Options
                            {
                                FolderPath = "D:\\cimplex\\",
                                IntervalInSeconds = 300
                            })
                    };
                }
            }
        }
    }
}
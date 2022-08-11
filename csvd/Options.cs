using CommandLine;

namespace csvd
{
    public class Options
    {
        [Option('p', "primary-key",
            Separator = ',',
            Default = new[] {0},
            HelpText = "Shared field(s) to use as primary key of csv files.")]
         public IEnumerable<int>? pKey { get; set; }

        [Option('e', "exclude-columns",
            Separator = ',',
            HelpText = "Columns to exclude from diff.")]
         public IEnumerable<int>? excludeCols { get; set; }

        [Value(0, MetaName = "Old csv file",
                HelpText = "Old file version",
                Required = true)]
        public string? OldFile { get; set; }

        [Value(1, MetaName = "new csv file",
                HelpText = "new file version",
                Required = true)]
         public string? NewFile { get; set; }
    }
}

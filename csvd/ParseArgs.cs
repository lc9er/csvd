using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace csvd
{
    public class Options
    {
        [Option('p', "primary-key",
            Separator = ',',
            HelpText = "Shared field(s) to use as primary key of csv files.")]
         public IEnumerable<int>? pKey { get; set; }

        /* [Option('e', "exclude-columns", */
        /*     HelpText = "Columns to exclude from diff.")] */
        /*  int?[] excludeCols { get; set; } */

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

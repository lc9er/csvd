using CommandLine;
using CommandLine.Text;

namespace csvd.UI.Options
{
    public class Options
    {
        [Option('p', "primary-key",
            Separator = ' ',
            Default = new[] { 0 },
            HelpText = "Single-space, 0-indexed, list of column numbers used to compare csv files.")]
        public IEnumerable<int>? pKey { get; set; }

        [Option('e', "exclude-columns",
            Separator = ' ',
            HelpText = "Single-space, 0-indexed, list of column numbers to exclude from comparison.")]
        public IEnumerable<int>? excludeCols { get; set; }

        [Option('d', "delimiter",
            Default = ',',
            HelpText = "Delimiter character. Defaults to comma.")]
        public char delimiter { get; set; }

        [Value(0, MetaName = "Old csv file",
                HelpText = "Old file version",
                Required = true)]
        public string? OldFile { get; set; }

        [Value(1, MetaName = "New csv file",
                HelpText = "New file version",
                Required = true)]
        public string? NewFile { get; set; }

        [Usage(ApplicationAlias = "csvd")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Compare two versions of a csv file. Defaults to column 0 as primary key",
                    new Options { OldFile = "OldFile.csv", NewFile = "NewFile.csv" });
                yield return new Example("Compare files with compound primary key and exclude columns",
                    new Options { OldFile = "OldFile.csv", NewFile = "NewFile.csv", pKey = new List<int>() { 0, 1 }, excludeCols = new List<int>() { 2, 7 } });
                yield return new Example("Compare files with custom delimiter",
                    new Options { OldFile = "OldFile.csv", NewFile = "NewFile.csv", delimiter = ':' });
            }
        }
        public static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "csvd 2.0.5";
                h.Copyright = "Copyright (c) 2023 lc9er";
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);
            Console.WriteLine(helpText);
        }
    }
}

using CommandLine;
using CommandLine.Text;

namespace csvd
{
    public class csvd
    {
        static void Main(string[] args)
        {
            var parser = new CommandLine.Parser(with => with.HelpWriter = null);
            var parserResults = parser.ParseArguments<Options>(args);
            parserResults
                .WithParsed<Options>(opts =>
                    {
                        Run(opts.OldFile, opts.NewFile, opts.pKey.ToList(), opts.excludeCols.ToList(), opts.delimiter);
                    })
                .WithNotParsed(errs => DisplayHelp(parserResults, errs));
        }

        static void Run(string OldFileName, string NewFileName, List<int> PrimaryKey, List<int> ExcludeFields, char delimiter)
        {
            // Instantiate ParseCsv objs
            var oldFileDict = new ParseCsv(OldFileName, delimiter, PrimaryKey, ExcludeFields);
            var newFileDict = new ParseCsv(NewFileName, delimiter, PrimaryKey, ExcludeFields);

            // create Dictionaries of pkey and csvrow values
            oldFileDict.SetCsvDict();
            newFileDict.SetCsvDict();

            // Find keys unique to each
            var oldFileDictUnique = oldFileDict.CsvFileDict.Keys.Except(newFileDict.CsvFileDict.Keys).ToList();
            var newFileDictUnique = newFileDict.CsvFileDict.Keys.Except(oldFileDict.CsvFileDict.Keys).ToList();

            // Find shared keys, with differences
            List<string> sharedKeys = oldFileDict.CsvFileDict.Keys.Intersect(newFileDict.CsvFileDict.Keys).ToList();

            // Find shared keys, with differing values
            var modifiedRows = oldFileDict.GetModifiedKeys(sharedKeys, newFileDict.CsvFileDict);

            // OutputTable
            var additions = new OutputTable($"Additions - ({newFileDictUnique.Count()})", TableType.ADDITION);
            additions.PrintSingleTable(newFileDictUnique, newFileDict);

            var modifications = new OutputTable($"Modifications - ({modifiedRows.Count()})", TableType.DIFFERENCE);
            modifications.PrintDifferenceTable(modifiedRows, oldFileDict, newFileDict);

            var removals = new OutputTable($"Removals - ({oldFileDictUnique.Count()})", TableType.REMOVAL);
            removals.PrintSingleTable(oldFileDictUnique, oldFileDict);
        }

        static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "csvd 1.0.0";
                h.Copyright = "Copyright (c) 2022 lc9er";
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);
            Console.WriteLine(helpText);
        }
    }
}

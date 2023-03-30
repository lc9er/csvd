using CommandLine;
using CommandLine.Text;
using csvd.Business;
using csvd.Business.Interfaces;
using csvd.Data;
using csvd.Data.csvd.Data;
using csvd.Domain.Model;
using csvd.UI.csvd.Options;
using csvd.UI.csvd.View;

namespace csvd;

public class Csvd
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
        // Instantiate csvd diff objs and data access
        ICsvd csvd = new CsvdService();
        IDataAccess dataAccess = new ParseCsv();

        var oldFile = new CsvFile(OldFileName, delimiter, PrimaryKey, ExcludeFields);
        var newFile = new CsvFile(NewFileName, delimiter, PrimaryKey, ExcludeFields);

        // create Dictionaries of pkey and csvrow values
        var oldFileDict = dataAccess.GetData(oldFile);
        var newFileDict = dataAccess.GetData(newFile);

        // Find keys unique to each
        List<string> oldFileDictUnique = csvd.GetUniqueKeys(oldFileDict.Keys.ToList(), newFileDict.Keys.ToList());
        List<string> newFileDictUnique = csvd.GetUniqueKeys(newFileDict.Keys.ToList(), oldFileDict.Keys.ToList());

        // Find shared keys, with differences
        List<string> sharedKeys = csvd.GetSharedKeys(oldFileDict.Keys.ToList(), newFileDict.Keys.ToList());

        // Find shared keys, with differing values
        // var modifiedRows = oldFileDict.GetModifiedKeys(sharedKeys, newFileDict.CsvFileDict);
        var modifiedRows = csvd.GetModifiedKeys(sharedKeys, oldFileDict, newFileDict);

        // OutputTable
        var additions = new OutputTable($"[blue]Additions - ({newFileDictUnique.Count})[/]", TableType.ADDITION);
        additions.PrintSingleTable(newFileDictUnique, newFileDict, newFile.header);

        var modifications = new OutputTable($"[red]Modifications - ({modifiedRows.Count})[/]", TableType.DIFFERENCE);
        modifications.PrintDifferenceTable(modifiedRows, oldFileDict, newFileDict, newFile.header);

        var removals = new OutputTable($"[orange1]Removals - ({oldFileDictUnique.Count})[/]", TableType.REMOVAL);
        removals.PrintSingleTable(oldFileDictUnique, oldFileDict, oldFile.header);
    }

    static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
    {
        var helpText = HelpText.AutoBuild(result, h =>
        {
            h.AdditionalNewLineAfterOption = false;
            h.Heading = "csvd 2.0.0";
            h.Copyright = "Copyright (c) 2022 lc9er";
            return HelpText.DefaultParsingErrorsHandler(result, h);
        }, e => e);
        Console.WriteLine(helpText);
    }
}

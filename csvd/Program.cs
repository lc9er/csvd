using CommandLine;
using csvd.Library;
using csvd.Library.Interfaces;
using csvd.Library.Model;
using csvd.UI.Options;
using csvd.UI.View;

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
            .WithNotParsed(errs => Options.DisplayHelp(parserResults, errs));
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
        IEnumerable<string> oldFileDictUnique = csvd.GetUniqueKeys(oldFileDict.csvDict.Keys, newFileDict.csvDict.Keys);
        IEnumerable<string> newFileDictUnique = csvd.GetUniqueKeys(newFileDict.csvDict.Keys, oldFileDict.csvDict.Keys);

        // Find shared keys, with differences
        IEnumerable<string> sharedKeys = csvd.GetSharedKeys(oldFileDict.csvDict.Keys, newFileDict.csvDict.Keys);

        // Find shared keys, with differing values
        var modifiedRows = csvd.GetModifiedKeys(sharedKeys, oldFileDict, newFileDict);

        // OutputTable
        var additions = new OutputTable($"[blue]Additions[/]", TableType.ADDITION);
        additions.PrintSingleTable(newFileDictUnique, newFileDict, newFile.header);

        var modifications = new OutputTable($"[red]Modifications[/]", TableType.DIFFERENCE);
        modifications.PrintDifferenceTable(modifiedRows, oldFileDict, newFileDict, newFile.header);

        var removals = new OutputTable($"[orange1]Removals[/]", TableType.REMOVAL);
        removals.PrintSingleTable(oldFileDictUnique, oldFileDict, oldFile.header);
    }
}

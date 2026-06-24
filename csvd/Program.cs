using CommandLine;
using csvd.Library;
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
                    Run(opts);
                })
            .WithNotParsed(errs => Options.DisplayHelp(parserResults, errs));
    }

    static void Run(Options opts)
    {
        // Instantiate csvd diff objs and data access
        ParseCsv dataAccess = new ();

        var oldFile = new CsvFile(opts.OldFile, opts.delimiter, opts.pKey, opts.excludeCols);
        var newFile = new CsvFile(opts.NewFile, opts.delimiter, opts.pKey, opts.excludeCols);

        // create Dictionaries of pkey and csvrow values
        var oldFileDict                = dataAccess.GetOriginalData(oldFile);
        var (newFileDict, modFileDict) = dataAccess.GetNewData(newFile, oldFileDict);

        // OutputTable
        var additions = new OutputTable($"[blue]Additions[/]", TableType.ADDITION);
        additions.PrintSingleTable(newFileDict, newFile.header);

        var modifications = new OutputTable($"[red]Modifications[/]", TableType.DIFFERENCE);
        modifications.PrintDifferenceTable(oldFileDict, modFileDict, newFileDict, newFile.header);

        var removals = new OutputTable($"[orange1]Removals[/]", TableType.REMOVAL);
        removals.PrintSingleTable(oldFileDict, oldFile.header);
    }
}

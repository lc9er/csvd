using System.CommandLine;
using csvd.Library;
using csvd.Library.Interfaces;
using csvd.Library.Model;
using csvd.UI.Options;
using csvd.UI.View;

namespace csvd.UI.Options;

using System.CommandLine;


public class Options
{
    public static RootCommand BuildRootCommand()
    {
        Argument<FileInfo> oldFile = new("oldfile")
        {
            Description = "Old file",
        };

        Argument<FileInfo> newFile = new("newfile")
        {
            Description = "New file",
        };

        Option<int[]> pKey = new("--primary-key", "-p")
        {
            Arity = ArgumentArity.OneOrMore,
            AllowMultipleArgumentsPerToken = true,
            Description = "Single-space, 0-indexed, list of column numbers used to compare csv files.",
            DefaultValueFactory = parseResult => [0],
        };

        Option<int[]> excludeCols = new("-e")
        {
            Arity = ArgumentArity.ZeroOrMore,
            AllowMultipleArgumentsPerToken = true,
            Description = "Single-space, 0-indexed, list of column numbers to exclude from comparison.",
        };

        Option<string> delimiter = new("--delimiter", "-d")
        {
            Description = "Delimiting character (wrapped in quotes)",
            DefaultValueFactory = parseResult => ",",
        };

        RootCommand rootCommand = new("cat a csv file");
        rootCommand.Arguments.Add(oldFile);
        rootCommand.Arguments.Add(newFile);
        rootCommand.Options.Add(pKey);
        rootCommand.Options.Add(excludeCols);
        rootCommand.Options.Add(delimiter);

        rootCommand.SetAction(parseResult =>
        {
            var  oldfile   = parseResult.GetValue(oldFile);
            var  newfile   = parseResult.GetValue(newFile);
            var  pkey      = parseResult.GetValue(pKey);
            var  exCols    = parseResult.GetValue(excludeCols);
            var  delimOpt  = parseResult.GetValue(delimiter);
            char delimChar = delimOpt[0];

            Run(oldfile.Name, newfile.Name, pkey, exCols, delimChar);
        });
    
        return rootCommand;
    }
    
    private static void Run(string oFile, string nFile, int[] pk, int[] exc, char delimChar)
    {
        // Instantiate csvd diff objs and data access
        IDataAccess dataAccess = new ParseCsv();

        var oldFile = new CsvFile(oFile, delimChar, pk, exc);
        var newFile = new CsvFile(nFile, delimChar, pk, exc);

        // create Dictionaries of pkey and csvrow values
        var oldFileDict = dataAccess.GetData(oldFile);
        var newFileDict = dataAccess.GetData(newFile);

        var (oldFileDictUnique, modifiedRows, newFileDictUnique) = oldFileDict.CompareTo(newFileDict);

        // OutputTable
        var additions = new OutputTable($"[blue]Additions[/]", TableType.ADDITION);
        additions.PrintSingleTable(newFileDictUnique, newFileDict, newFile.header);

        var modifications = new OutputTable($"[red]Modifications[/]", TableType.DIFFERENCE);
        modifications.PrintDifferenceTable(modifiedRows, oldFileDict, newFileDict, newFile.header);

        var removals = new OutputTable($"[orange1]Removals[/]", TableType.REMOVAL);
        removals.PrintSingleTable(oldFileDictUnique, oldFileDict, oldFile.header);
    }
}
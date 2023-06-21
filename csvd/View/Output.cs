using csvd.Library.Model;
using Spectre.Console;
using System.Security.Cryptography;

namespace csvd.UI.View;

public enum TableType
{
    ADDITION,
    REMOVAL,
    DIFFERENCE
}

public class OutputTable
{
    public Table table = new();
    public TableType tableColor;

    public OutputTable(string title, TableType tableStyle)
    {
        table.Title(title);
        table.Border = TableBorder.Simple;
        tableColor   = tableStyle;
    }

    public static Markup[] FormatTableRow(string color, List<string> row)
    {
        int rowSize = row.Count;
        var formattedRow = new Markup[rowSize];

        for (int i = 0; i < rowSize; i++)
            formattedRow[i] = new Markup(color + Markup.Escape(row[i]) + "[/]");

        return formattedRow;
    }

    // Overloaded version for cell diffs
    public static Markup[] FormatTableRow(string color, List<string> row, List<int> diffs)
    {
        int rowSize = row.Count;
        var formattedRow = new Markup[rowSize];

        for (int i = 0; i < rowSize; i++)
        {
            string styledColor = diffs.Contains(i) ? "[red]" : color;
            formattedRow[i] = new Markup(styledColor + Markup.Escape(row[i]) + "[/]");
        }

        return formattedRow;
    }

    private static List<int> FindRowDiffereces(List<string> oldRow, List<string> newRow) =>
        Enumerable.Range(0, oldRow.Count)
            .Where(x => oldRow[x] != newRow[x])
            .ToList();

    public void PrintDifferenceTable(IEnumerable<string> modifiedKeys, CsvDict oldCsv,
        CsvDict newCsv, HeaderRow header)
    {

        table.AddColumns(header.Header.ToArray());

        foreach (var key in modifiedKeys)
        {
            var oldRow = oldCsv.csvDict[key].ToList();
            var newRow = newCsv.csvDict[key].ToList();

            var diffs = FindRowDiffereces(oldRow, newRow);
            table.AddRow(FormatTableRow("[orange1]", oldRow, diffs));
            table.AddRow(FormatTableRow("[blue]", newRow, diffs));
        }

        AnsiConsole.Write(table);
    }

    public void PrintSingleTable(IEnumerable<string> keys, CsvDict CsvObj, HeaderRow header)
    {

        // Build, but hide header columns
        table.AddColumns(header.Header.ToArray());

        string cellColor = tableColor switch
        {
            TableType.ADDITION  => "[blue]",
            TableType.REMOVAL   => "[orange1]",
            _                   => "[white]",
        };

        foreach (var key in keys)
            table.AddRow(FormatTableRow(cellColor, CsvObj.csvDict[key].ToList()));

        AnsiConsole.Write(table);
    }
}

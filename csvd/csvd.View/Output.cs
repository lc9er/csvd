using Spectre.Console;

namespace csvd.UI.csvd.View;

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
        tableColor = tableStyle;
    }

    public Markup[] FormatTableRow(string color, List<string> row)
    {
        int rowSize = row.Count;
        var formattedRow = new Markup[rowSize];

        for (int i = 0; i < rowSize; i++)
            formattedRow[i] = new Markup(color + Markup.Escape(row[i]) + "[/]");

        return formattedRow;
    }

    // Overloaded version for cell diffs
    public Markup[] FormatTableRow(string color, List<string> row, List<int> diffs)
    {
        int rowSize = row.Count;
        var formattedRow = new Markup[rowSize];

        for (int i = 0; i < rowSize; i++)
        {
            string styledColor;

            // make cells that differ red 
            if (diffs.Contains(i))
                styledColor = "[red]";
            else
                styledColor = color;

            formattedRow[i] = new Markup(styledColor + Markup.Escape(row[i]) + "[/]");
        }

        return formattedRow;
    }

    private List<int> FindRowDiffereces(List<string> oldRow, List<string> newRow)
    {
        int rowSize = oldRow.Count;
        List<int> cellDiffs = new List<int>();

        for (int i = 0; i < rowSize; i++)
            if (!oldRow[i].Equals(newRow[i]))
                cellDiffs.Add(i);

        return cellDiffs;
    }

    public void PrintDifferenceTable(List<string> modifiedKeys, Dictionary<string, List<string>> oldCsv,
        Dictionary<string, List<string>> newCsv, List<string> header)
    {

        table.AddColumns(header.ToArray());

        foreach (var key in modifiedKeys)
        {
            var oldRow = oldCsv[key].ToList();
            var newRow = newCsv[key].ToList();

            var diffs = FindRowDiffereces(oldRow, newRow);
            table.AddRow(FormatTableRow("[orange1]", oldRow, new List<int>()));
            table.AddRow(FormatTableRow("[blue]", newRow, new List<int>()));
        }

        AnsiConsole.Write(table);
    }

    public void PrintSingleTable(IEnumerable<string> keys, Dictionary<string, List<string>> CsvObj, List<string> header)
    {

        // Build, but hide header columns
        table.AddColumns(header.ToArray());

        string cellColor = tableColor switch
        {
            TableType.ADDITION  => "[blue]",
            TableType.REMOVAL   => "[orange1]",
            _                   => "[white]",
        };

        foreach (var key in keys)
            table.AddRow(FormatTableRow(cellColor, CsvObj[key].ToList()));

        AnsiConsole.Write(table);
    }
}

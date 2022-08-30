using Spectre.Console;

namespace csvd
{
    public enum TableType
    {
        ADDITION,
        REMOVAL,
        DIFFERENCE
    }

    class OutputTable
    {
        public Table table = new Table();
        public TableType tableColor;

        public OutputTable(string title, TableType tableStyle)
        {
            table.Title(title);
            tableColor = tableStyle;
        }

        public string[] FormatTableRow(string color, List<string> row)
        {
            var formattedRow = new List<string>();

            foreach (var cell in row)
                formattedRow.Add(color + cell + "[/]");

            return formattedRow.ToArray();
        }

        public void PrintDifferenceTable(List<string> modifiedKeys, ParseCsv oldCsv, ParseCsv newCsv)
        {
            table.AddColumns(oldCsv.header.ToArray());

            foreach (var key in modifiedKeys)
            {
                var oldRow = oldCsv.csvFileDict[key].ToList();
                table.AddRow(FormatTableRow("[orange1]", oldRow));

                var newRow = newCsv.csvFileDict[key].ToList();
                table.AddRow(FormatTableRow("[blue]", newRow));
            }

            AnsiConsole.Write(table);
        }

        public void PrintSingleTable(IEnumerable<string> keys, ParseCsv CsvObj)
        {
            string cellColor;

            // Build, but hide header columns
            table.AddColumns(CsvObj.header.ToArray());

            // value coloring
            switch (tableColor)
            {
                case TableType.ADDITION:
                    cellColor = "[blue]";
                    break;
                case TableType.REMOVAL:
                    cellColor = "[orange1]";
                    break;
                default:
                    cellColor = "[white]";
                    break;
            }

            foreach (var key in keys)
            {
                var row = CsvObj.csvFileDict[key].ToList();
                table.AddRow(FormatTableRow(cellColor, row));
            }

            AnsiConsole.Write(table);
        }
    }
}

using Spectre.Console;

namespace csvd
{
    public enum TableType
    {
        ADDITION,
        REMOVAL,
        DIFFERENCE
    }

    public class OutputTable
    {
        public Table table = new Table();
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
            {
                formattedRow[i] = new Markup(color + Markup.Escape(row[i]) + "[/]");
            }

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

                // add bold to cells that differ 
                if (diffs.Contains(i))
                {
                    styledColor = "[red]";
                }
                else
                {
                    styledColor = color;
                }

                formattedRow[i] = new Markup(styledColor + Markup.Escape(row[i]) + "[/]");
            }

            return formattedRow;
        }

        private List<int> FindRowDiffereces(List<string> oldRow, List<string> newRow)
        {
            int rowSize = oldRow.Count;
            List<int> cellDiffs = new List<int>();

            for (int i = 0; i < rowSize; i++)
            {
                if (!oldRow[i].Equals(newRow[i]))
                {
                    cellDiffs.Add(i);
                }
            }

            return cellDiffs;
        }

        public void PrintDifferenceTable(List<string> modifiedKeys, ParseCsv oldCsv, ParseCsv newCsv)
        {

            table.AddColumns(oldCsv.Header.ToArray());

            foreach (var key in modifiedKeys)
            {
                List<int> diffs = new List<int>();
                var oldRow = oldCsv.CsvFileDict[key].ToList();
                var newRow = newCsv.CsvFileDict[key].ToList();

                diffs = FindRowDiffereces(oldRow, newRow);
                table.AddRow(FormatTableRow("[orange1]", oldRow, diffs));
                table.AddRow(FormatTableRow("[blue]", newRow, diffs));
            }

            AnsiConsole.Write(table);
        }

        public void PrintSingleTable(IEnumerable<string> keys, ParseCsv CsvObj)
        {
            string cellColor;

            // Build, but hide header columns
            table.AddColumns(CsvObj.Header.ToArray());

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
                var row = CsvObj.CsvFileDict[key].ToList();
                table.AddRow(FormatTableRow(cellColor, row));
            }

            AnsiConsole.Write(table);
        }
    }
}

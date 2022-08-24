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

        public void PrintDifferenceTable(List<string> modifiedKeys, ParseCsv oldCsv, ParseCsv newCsv)
        {
            int rowCount = 0;

            table.AddColumns(oldCsv.header.ToArray());

            foreach (var key in modifiedKeys)
            {
                var oldRow = oldCsv.CsvFileDict[key].ToList();
                var oldOutputRow = new List<string>();

                foreach (var cell in oldRow)
                {
                    oldOutputRow.Add("[orange1]" + cell + "[/]");
                }

                table.AddRow(oldOutputRow.ToArray());

                var newRow = newCsv.CsvFileDict[key].ToList();
                var newOutputRow = new List<string>();

                foreach (var cell in newRow)
                {
                    newOutputRow.Add("[blue]" + cell + "[/]");
                }
                
                table.AddRow(newOutputRow.ToArray());
            }

            AnsiConsole.Write(table);
        }

        public void PrintSingleTable(IEnumerable<string> keys, ParseCsv CsvObj)
        {
            string cellColor;

            // Build, but hide header columns
            table.AddColumns(CsvObj.header.ToArray());
            /* foreach (var col in CsvObj.header) */
            /* { */
            /*     table.AddColumn(col); */
            /* } */

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
                var outputRow = new List<string>();

                foreach (var cell in row)
                {
                    outputRow.Add(cellColor + cell + "[/]");
                }

                table.AddRow(outputRow.ToArray());
            }

            AnsiConsole.Write(table);
        }
    }
}

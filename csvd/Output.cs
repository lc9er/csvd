using Spectre.Console;
using System.Collections.Generic;

namespace csvd
{
    class OutputTable
    {
        public Table table = new Table();
        
        public OutputTable(string title)
        {
            table.Title(title);
        }
        
        public void PrintSingleTable(IEnumerable<string> keys, ParseCsv CsvObj)
        {
            // Build, but hide header columns
            foreach (var col in CsvObj.header)
            {
                table.AddColumn(col);
            }
            table.HideHeaders();

            foreach(var key in keys)
            {
                table.AddRow(CsvObj.CsvFileDict[key].ToArray());
            }

            AnsiConsole.Write(table);
        }
    }
}

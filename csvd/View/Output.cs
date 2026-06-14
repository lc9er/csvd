using csvd.Library.Model;

namespace csvd.UI.View;

public class Output
{
    /// <summary>
    /// Prints header and rows removed from old file
    /// </summary>
    /// <param name="CsvObj"></param>
    /// <param name="header"></param>
    public static void PrintResults(CsvDict CsvObj, HeaderRow header)
    {
        header.Header.Insert(0, "Action");
        var headerRow = string.Join(",", header.Header);
        Console.WriteLine(headerRow);

        foreach (var key in CsvObj.csvDict.Keys)
        {
            var row = CsvObj.csvDict[key].ToList();
            row.Insert(0, "removed");
            Console.WriteLine(string.Join(",", row));
        }
    }

    /// <summary>
    /// Prints results for rows added to new file.
    /// </summary>
    /// <param name="CsvObj"></param>
    public static void PrintResults(CsvDict CsvObj)
    {
        foreach (var key in CsvObj.csvDict.Keys)
        {
            var row = CsvObj.csvDict[key].ToList();
            row.Insert(0, "added");
            Console.WriteLine(string.Join(",", row));

        }
    }

    /// <summary>
    /// Prints results for modified rows
    /// </summary>
    /// <param name="modifiedCsvDicts"></param>
    public static void PrintResults(List<ModifiedCsvDict> modifiedCsvDicts)
    {
        foreach (var modifiedRow in modifiedCsvDicts)
        {
            var oldRow = modifiedRow.OldRow.ToList();
            oldRow.Insert(0, "Old-Value");
            var newRow = modifiedRow.NewRow.ToList();
            newRow.Insert(0, "New-Value");

            Console.WriteLine(string.Join(",", oldRow)); 
            Console.WriteLine(string.Join(",", newRow)); 
        }
    }
}

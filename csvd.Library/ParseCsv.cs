using csvd.Library.Interfaces;
using csvd.Library.Model;
using FourLambda.Csv;
// using Sylvan.Data.Csv;

namespace csvd.Library;

public class ParseCsv : IDataAccess
{
    public CsvDict GetData(CsvFile csvFile)
    {
        var csvDict = new CsvDict();

        try
        {
            // var csvOpts = new CsvDataReaderOptions { Delimiter = csvFile.delimiter.DelimChar};
            // using CsvDataReader csv = CsvDataReader.Create(csvFile.fileName.Filename, csvOpts);
            using var csv = FourLambda.Csv.CsvReader.Create(csvFile.fileName.Filename, hasHeaders: true, separatorChar: csvFile.delimiter.DelimChar);

            [] = csv.Headers.Keys;
            // capture header row, minus excludes
            for (int i = 0; i < csv.FieldCount; i++)
                if (!csvFile.excludeFields.Exclude.Contains(i))
                    
                    csvFile.header.Header.Add(csv.Headers.Try);

            while (csv.ReadNext())
            {
                // Get Primary Key, and csv row values
                string pKey = GetPrimaryKey(csv, csvFile.primaryKey);
                IEnumerable<string> CsvRowValues = GetCsvFields(csv, csvFile.excludeFields);
                try
                {
                    csvDict.csvDict.Add(pKey, CsvRowValues);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine($"Duplicate primary key {pKey} found in {csvFile.fileName.Filename}.");
                    Environment.Exit(1);
                }
            }
        }

        catch (FileNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }

        return csvDict;
    }

    // NOTE: This adds 16% to runtime
    // public static string GetPrimaryKey(CsvDataReader line, List<int> primaryKey) =>
    //    string.Concat(primaryKey.Select(line.GetString));
    public static string GetPrimaryKey(FourLambda.Csv.CsvReaderUtf16 line, PrimaryKey primaryKey)
    {
        string pKey = null;

        foreach (var key in primaryKey.PKey)
            pKey += line.GetString(key);

        return pKey;
    }

    // LINQ here slows down 15%+
    public static IEnumerable<string> GetCsvFields(FourLambda.Csv.CsvReaderUtf16 line, ExcludeFields excludeFields)
    {
        int rowSize = line.FieldCount - excludeFields.Exclude.Length;
        string[] CsvValues = new string[rowSize];

        // populate CsvValues
        int index = 0;
        for (int i = 0; i < line.FieldCount; i++)
            if (!excludeFields.Exclude.Contains(i))
            {
                CsvValues[index] = line.GetString(i);
                index++;
            }

        return CsvValues.ToList();
    }
}

using csvd.Data.csvd.Data;
using csvd.Domain.Model;
using Sylvan.Data.Csv;
using System.Text;

namespace csvd.Data;

public class ParseCsv : IDataAccess
{
    public Dictionary<string, List<string>> GetData(CsvFile csvFile)
    {
        var CsvDict = new Dictionary<string, List<string>>();

        try
        {
            var csvOpts = new CsvDataReaderOptions { Delimiter = csvFile.delimiter };
            using CsvDataReader csv = CsvDataReader.Create(csvFile.fileName, csvOpts);

            // capture header row, minus excludes
            for (int i = 0; i < csv.FieldCount; i++)
                if (!csvFile.excludeFields.Contains(i))
                    csvFile.header.Add(csv.GetName(i));

            while (csv.Read())
            {
                // Get Primary Key, and csv row values
                string pKey = GetPrimaryKey(csv, csvFile.primaryKey);
                List<string> CsvRowValues = GetCsvFields(csv, csvFile.excludeFields);
                try
                {
                    CsvDict.Add(pKey, CsvRowValues);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine($"Duplicate primary key {pKey} found in {csvFile.fileName}.");
                    Environment.Exit(1);
                }
            }
        }

        catch (FileNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }

        return CsvDict;
    }
    public string GetPrimaryKey(CsvDataReader line, List<int> primaryKey)
    {
        var pKey = new StringBuilder();

        foreach (var key in primaryKey)
            pKey.Append(line.GetString(key));

        return pKey.ToString();
    }

    public List<string> GetCsvFields(CsvDataReader line, List<int> excludeFields)
    {
        var CsvValues = new List<string>();

        for (int i = 0; i < line.FieldCount; i++)
            if (!excludeFields.Contains(i))
                CsvValues.Add(line.GetString(i));

        return CsvValues.ToList();
    }
}

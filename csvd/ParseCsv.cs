using CsvHelper;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace csvd
{
    // how to get fields, minus exclusions
    //
    // List<int> excludeFields = new List<int> { 1, 3 }
    // List<string> csvRow = new List<string> { "First", "Second", "third", "fourth", "fifth" }
    //
    // var filteredCsvRow = csvRow.Where((field, index) => !(excludeFields.Contains(index)));
    // can convert to list or create directly:
    // List<string> filteredCsvRow = csvRow.Where((field, index) => !(excludeFields.Contains(index))).ToList();
    // csvDict.Add(csvRow[primaryKey], filteredCsvRow);
    //
    // Printing out/accessing the dictionary
    // new Dictionary<string, List<string>> csvDict = new Dictionary<string, List<string>>()
    public static class ParseCsv
    {
        public static string GetPrimaryKey(CsvReader line, IEnumerable<int> PrimaryKey)
        {
            string pKey = "";
            string pKeyField;

            foreach (var key in PrimaryKey)
            {
                line.TryGetField(key, out pKeyField);
                pKey += pKeyField;
            }

            return pKey;
        }

        public static List<string> GetCsvFields(CsvReader line)
        {
            List<string> CsvFieldValues = new List<string>();
            string field;
            
            for (int i = 0; line.TryGetField<string>(i, out field); i++)
            {
                /* line.TryGetField<string>(i, out field); */
                CsvFieldValues.Add(field);
            }

            return CsvFieldValues;
        }

        public static Dictionary<string, List<string>> GetCsvDict(string FilePath, IEnumerable<int> PrimaryKey)
        {
            Dictionary<string, List<string>> CsvDict = new Dictionary<string, List<string>>();

            try 
            {
                var stream = File.ReadAllLines(FilePath);

                foreach (var line in stream)
                {
                    // Build dictionary, line by line
                    using (var reader = new StringReader(line))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        while(csv.Read())
                        {
                            // Get Primary Key
                            string pKey = GetPrimaryKey(csv, PrimaryKey);
                            List<string> CsvRowValues = GetCsvFields(csv);
                            
                            CsvDict.Add(pKey, CsvRowValues);
                        }
                    }
                }
            }

            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return CsvDict;
        }
    }
}

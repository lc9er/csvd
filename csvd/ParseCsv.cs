using Sylvan.Data.Csv;
using System.Text;

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
        public static string GetPrimaryKey(CsvDataReader line, IEnumerable<int> PrimaryKey)
        {
            var pKey = new StringBuilder();

            foreach (var key in PrimaryKey)
                pKey.Append(line.GetString(key));

            return pKey.ToString();
        }

        public static IEnumerable<string> GetCsvFields(CsvDataReader line)
        {
            var CsvValues = new List<string>();

            for (int i = 0; i < line.FieldCount; i++)
                CsvValues.Add(line.GetString(i));

            return CsvValues;
        }

        public static Dictionary<string, List<string>> GetCsvDict(string FilePath, IEnumerable<int> PrimaryKey)
        {
            var CsvDict = new Dictionary<string, List<string>>();

            try 
            {
                using CsvDataReader csv = CsvDataReader.Create(FilePath);
                while(csv.Read())
                {
                    // Get Primary Key, and csv row values
                    string pKey = GetPrimaryKey(csv, PrimaryKey);
                    List<string> CsvRowValues = GetCsvFields(csv).ToList();
                    CsvDict.Add(pKey, CsvRowValues);
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

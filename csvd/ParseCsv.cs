using Sylvan.Data.Csv;
using System.Text;

namespace csvd
{
    public static class ParseCsv
    {
        public static string GetPrimaryKey(CsvDataReader line, IEnumerable<int> PrimaryKey)
        {
            var pKey = new StringBuilder();

            foreach (var key in PrimaryKey)
                pKey.Append(line.GetString(key));

            return pKey.ToString();
        }

        public static IEnumerable<string> GetCsvFields(CsvDataReader line, IEnumerable<int> ExcludeFields)
        {
            var CsvValues = new List<string>();

            for (int i = 0; i < line.FieldCount; i++)
            {
                if(!ExcludeFields.Contains(i))
                {
                    CsvValues.Add(line.GetString(i));
                }
            }

            return CsvValues;
        }

        public static Dictionary<string, List<string>> GetCsvDict(string FilePath, IEnumerable<int> PrimaryKey, IEnumerable<int> ExcludeFields)
        {
            var CsvDict = new Dictionary<string, List<string>>();

            try 
            {
                using CsvDataReader csv = CsvDataReader.Create(FilePath);
                while(csv.Read())
                {
                    // Get Primary Key, and csv row values
                    string pKey = GetPrimaryKey(csv, PrimaryKey);
                    List<string> CsvRowValues = GetCsvFields(csv, ExcludeFields).ToList();
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

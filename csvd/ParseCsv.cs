using Sylvan.Data.Csv;
using System.Text;

namespace csvd
{
    public class ParseCsv
    {
        public string fileName;
        public Dictionary<string, List<string>> CsvFileDict = new Dictionary<string, List<string>>();

        public ParseCsv(string FileName)
        {
            fileName = FileName;
        }

        public string GetPrimaryKey(CsvDataReader line, IEnumerable<int> PrimaryKey)
        {
            var pKey = new StringBuilder();

            foreach (var key in PrimaryKey)
                pKey.Append(line.GetString(key));

            return pKey.ToString();
        }

        public IEnumerable<string> GetCsvFields(CsvDataReader line, IEnumerable<int> ExcludeFields)
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

        public void SetCsvDict(IEnumerable<int> PrimaryKey, IEnumerable<int> ExcludeFields)
        {
            var CsvDict = new Dictionary<string, List<string>>();

            try 
            {
                using CsvDataReader csv = CsvDataReader.Create(fileName);
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

            CsvFileDict = CsvDict;
        }
    }
}

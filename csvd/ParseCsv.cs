using Sylvan.Data.Csv;
using System.Text;

namespace csvd
{
    public class ParseCsv
    {
        public string fileName;
        public char delimiter;
        public Dictionary<string, List<string>> CsvFileDict = new Dictionary<string, List<string>>();

        public ParseCsv(string FileName, char delimChar)
        {
            fileName = FileName;
            delimiter = delimChar;
        }

        public List<string> GetModifiedKeys(IEnumerable<string> sharedKeys,
                Dictionary<string, List<string>> newFileDict)
        {
            List<string> modifiedKeys = new List<string>();

            foreach (var key in sharedKeys)
            {
                List<string> oldVals = CsvFileDict[key];
                List<string> newVals = newFileDict[key];

                if (!(oldVals.SequenceEqual(newVals)))
                {
                    modifiedKeys.Add(key);
                }
            }

            return modifiedKeys;
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
                var csvOpts = new CsvDataReaderOptions { Delimiter = delimiter };
                using CsvDataReader csv = CsvDataReader.Create(fileName, csvOpts);
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

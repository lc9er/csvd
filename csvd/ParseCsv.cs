using Sylvan.Data.Csv;
using System.Text;

namespace csvd
{
    public class ParseCsv
    {
        public string fileName;
        public char delimiter;
        public IEnumerable<int> primaryKey;
        public IEnumerable<int> excludeFields;
        public Dictionary<string, List<string>> csvFileDict = new Dictionary<string, List<string>>();
        public List<string> header = new List<string>();

        public ParseCsv(string FileName, char DelimChar, IEnumerable<int> PrimaryKey, IEnumerable<int> ExcludeFields)
        {
            fileName = FileName;
            delimiter = DelimChar;
            primaryKey = PrimaryKey;
            excludeFields = ExcludeFields;
        }

        public List<string> GetModifiedKeys(IEnumerable<string> sharedKeys,
                Dictionary<string, List<string>> newFileDict)
        {
            List<string> modifiedKeys = new List<string>();

            foreach (var key in sharedKeys)
            {
                List<string> oldVals = csvFileDict[key];
                List<string> newVals = newFileDict[key];

                if (!(oldVals.SequenceEqual(newVals)))
                    modifiedKeys.Add(key);
            }

            return modifiedKeys;
        }

        public string GetPrimaryKey(CsvDataReader line)
        {
            var pKey = new StringBuilder();

            foreach (var key in primaryKey)
                pKey.Append(line.GetString(key));

            return pKey.ToString();
        }

        public IEnumerable<string> GetCsvFields(CsvDataReader line)
        {
            var CsvValues = new List<string>();

            for (int i = 0; i < line.FieldCount; i++)
                if (!excludeFields.Contains(i))
                    CsvValues.Add(line.GetString(i));

            return CsvValues;
        }

        public void SetCsvDict()
        {
            var CsvDict = new Dictionary<string, List<string>>();

            try
            {
                var csvOpts = new CsvDataReaderOptions { Delimiter = delimiter };
                using CsvDataReader csv = CsvDataReader.Create(fileName, csvOpts);

                // capture header row, minus excludes
                for (int i = 0; i < csv.FieldCount; i++)
                    if (!excludeFields.Contains(i))
                        header.Add(csv.GetName(i));

                while (csv.Read())
                {
                    // Get Primary Key, and csv row values
                    string pKey = GetPrimaryKey(csv);
                    List<string> CsvRowValues = GetCsvFields(csv).ToList();
                    CsvDict.Add(pKey, CsvRowValues);
                }
            }

            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }

            csvFileDict = CsvDict;
        }
    }
}

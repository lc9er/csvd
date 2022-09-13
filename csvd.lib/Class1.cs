using Sylvan.Data.Csv;
using System.Text;

namespace csvd
{
    public class ParseCsv
    {
        private string fileName;
        private char delimiter;
        private List<int> primaryKey;
        private List<int> excludeFields;
        private Dictionary<string, List<string>> csvFileDict = new Dictionary<string, List<string>>();
        private List<string> header = new List<string>();

        public List<string> Header
        {
            get
            {
                return header;
            }
        }

        public Dictionary<string, List<string>> CsvFileDict
        {
            get
            {
                return csvFileDict;
            }
        }

        public ParseCsv(string FileName, char DelimChar, List<int> PrimaryKey, List<int> ExcludeFields)
        {
            fileName = FileName;
            delimiter = DelimChar;
            primaryKey = PrimaryKey;
            excludeFields = ExcludeFields;
        }

        public List<string> GetModifiedKeys(List<string> sharedKeys,
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

        public List<string> GetCsvFields(CsvDataReader line)
        {
            var CsvValues = new List<string>();

            for (int i = 0; i < line.FieldCount; i++)
                if (!excludeFields.Contains(i))
                    CsvValues.Add(line.GetString(i));

            return CsvValues.ToList();
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
                    List<string> CsvRowValues = GetCsvFields(csv);
                    try
                    {
                        CsvDict.Add(pKey, CsvRowValues);
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine($"Duplicate primary key {pKey} found in {fileName}.");
                        Environment.Exit(1);
                    }
                }
            }

            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }

            csvFileDict = CsvDict;
        }
    }
}

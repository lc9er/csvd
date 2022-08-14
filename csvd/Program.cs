using CommandLine;

namespace csvd
{
    public class csvd
    {
        static void Main(string[] args)
        {
            var parserResults = Parser.Default.ParseArguments<Options>(args);
            parserResults.WithParsed<Options>(opts =>
                    {
                        Run(opts.OldFile, opts.NewFile, opts.pKey, opts.excludeCols);
                    });
        }

        static List<string> GetModifiedKeys(IEnumerable<string> sharedKeys,
                Dictionary<string, List<string>> oldFileDict,
                Dictionary<string, List<string>> newFileDict)
        {
            List<string> modifiedKeys = new List<string>();

            foreach (var key in sharedKeys)
            {
                List<string> oldVals = oldFileDict[key];
                List<string> newVals = newFileDict[key];

                if (!(oldVals.SequenceEqual(newVals)))
                {
                    modifiedKeys.Add(key);
                }
            }

            return modifiedKeys;
        }

        static void Run(string OldFileName, string NewFileName, IEnumerable<int> PrimaryKey, IEnumerable<int> ExcludeFields)
        {
            // Instantiate ParseCsv objs
            var oldFileDict = new ParseCsv(OldFileName);
            var newFileDict = new ParseCsv(NewFileName);

            // create Dictionaries of pkey and csvrow values
            oldFileDict.SetCsvDict(PrimaryKey, ExcludeFields);
            newFileDict.SetCsvDict(PrimaryKey, ExcludeFields);

            // Find keys unique to each
            var oldFileDictUnique = oldFileDict.CsvFileDict.Keys.Except(newFileDict.CsvFileDict.Keys);
            var newFileDictUnique = newFileDict.CsvFileDict.Keys.Except(oldFileDict.CsvFileDict.Keys);

            // Find shared keys, with differences
            IEnumerable<string> both = oldFileDict.CsvFileDict.Keys.Intersect(newFileDict.CsvFileDict.Keys);

            // Find shared keys, with differing values
            var modifiedRows = GetModifiedKeys(both, oldFileDict.CsvFileDict, newFileDict.CsvFileDict);
            foreach (var mod in modifiedRows)
            {
                Console.WriteLine($"Old File: ");
                foreach (var val in oldFileDict.CsvFileDict[mod])
                    Console.Write($"{val},");
                Console.WriteLine();
                Console.WriteLine($"New File: ");
                foreach (var val in newFileDict.CsvFileDict[mod])
                    Console.Write($"{val},");
                Console.WriteLine();
            }
        }
    }
}

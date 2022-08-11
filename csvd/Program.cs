using CommandLine;

namespace csvd
{
    public class csvd
    {
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
            // create Dictionaries of pkey and csvrow values
            Dictionary<string, List<string>> oldFileDict = ParseCsv.GetCsvDict(OldFileName, PrimaryKey, ExcludeFields);
            Dictionary<string, List<string>> newFileDict = ParseCsv.GetCsvDict(NewFileName, PrimaryKey, ExcludeFields);

            // Find keys unique to each
            var oldFileDictUnique = oldFileDict.Keys.Except(newFileDict.Keys);
            var newFileDictUnique = newFileDict.Keys.Except(oldFileDict.Keys);

            // Find shared keys, with differences
            IEnumerable<string> both = oldFileDict.Keys.Intersect(newFileDict.Keys);

            // Find shared keys, with differing values
            var modifiedRows = GetModifiedKeys(both, oldFileDict, newFileDict);
            foreach (var mod in modifiedRows)
            {
                Console.WriteLine($"Old File: ");
                foreach (var val in oldFileDict[mod])
                    Console.Write($"{val},");
                Console.WriteLine();
                Console.WriteLine($"New File: ");
                foreach (var val in newFileDict[mod])
                    Console.Write($"{val},");
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            var parserResults = Parser.Default.ParseArguments<Options>(args);
            parserResults.WithParsed<Options>(opts =>
                    {
                        Run(opts.OldFile, opts.NewFile, opts.pKey, opts.excludeCols);
                    });
        }
    }
}

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
                        Run(opts.OldFile, opts.NewFile, opts.pKey, opts.excludeCols, opts.delimiter);
                    });
        }

        static void Run(string OldFileName, string NewFileName, IEnumerable<int> PrimaryKey, IEnumerable<int> ExcludeFields, char delimiter)
        {
            // Instantiate ParseCsv objs
            var oldFileDict = new ParseCsv(OldFileName, delimiter);
            var newFileDict = new ParseCsv(NewFileName, delimiter);

            // create Dictionaries of pkey and csvrow values
            oldFileDict.SetCsvDict(PrimaryKey, ExcludeFields);
            newFileDict.SetCsvDict(PrimaryKey, ExcludeFields);

            // Find keys unique to each
            var oldFileDictUnique = oldFileDict.CsvFileDict.Keys.Except(newFileDict.CsvFileDict.Keys);
            var newFileDictUnique = newFileDict.CsvFileDict.Keys.Except(oldFileDict.CsvFileDict.Keys);

            // Find shared keys, with differences
            IEnumerable<string> sharedKeys = oldFileDict.CsvFileDict.Keys.Intersect(newFileDict.CsvFileDict.Keys);

            // Find shared keys, with differing values
            var modifiedRows = oldFileDict.GetModifiedKeys(sharedKeys, newFileDict.CsvFileDict);
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

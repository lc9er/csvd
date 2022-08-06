using CommandLine;

namespace csvd 
{
    public class csvd
    {
        static void Run(string OldFileName, string NewFileName, IEnumerable<int> PrimaryKey)
        {
            // Get CsvDict for old file
            Dictionary<string, List<string>> oldFileDict = ParseCsv.GetCsvDict(OldFileName, PrimaryKey);
            Console.WriteLine();
            Console.WriteLine($"OldFile: {OldFileName}");
            foreach (var key in oldFileDict.Keys)
            {
                Console.WriteLine($"Key: {key}");
                List<string> values = oldFileDict[key];
                foreach (var value in values)
                {
                    Console.WriteLine($"Key: {key}, Value: {value}");
                }
            }

            // Get CsvDict for new file
            Dictionary<string, List<string>> newFileDict = ParseCsv.GetCsvDict(NewFileName, PrimaryKey);
            Console.WriteLine();
            Console.WriteLine($"New File: {NewFileName}");
            foreach (var key in newFileDict.Keys)
            {
                Console.WriteLine($"Key: {key}");
                List<string> values = newFileDict[key];
                foreach (var value in values)
                {
                    Console.WriteLine($"Key: {key}, Value: {value}");
                }
            }

            // Display primary keys
            if (PrimaryKey.Any() == true)
            {
                Console.WriteLine("Primary Key(s): ");
                foreach (var key in PrimaryKey)
                    Console.Write($"{key} ");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Primary Key: 0 (Default Value)");
            }

        }

        static void Main(string[] args)
        {
            var parserResults = Parser.Default.ParseArguments<Options>(args);
            parserResults.WithParsed<Options>( opts => 
                    {
                        Run(opts.OldFile, opts.NewFile, opts.pKey);
                    });
        }
        /* public static Dictionary<string, string> CreateCsvDict(string file, int key) */
        /* { */
        /*     // Read csv file. Build dictionary with key */
        /* } */

        /* public static string[] GetKeys(Dictionary<string, string> csvDict) */
        /* { */
        /*     // return the keys in the dictionary */
        /* } */

        /* public static string[] GetExclusiveKeys(Dictionary<string, string> csvDict1, Dictionary<string, string> csvDict2) */
        /* { */
        /*     // return keys exclusive to csvDict1 */
        /*     // Something like */ 
        /* } */

        /* static void Main(string[] args) */
        /* { */
            /// Parse args - file1 file2 key
            /* string[] files = { args[0], args[1] }; */
            /* int pKey = Int32.Parse(args[2]); */
            /* ParseArgs parsedArgs = new ParseArgs(args); */
            /* Console.WriteLine($"Old File: {parsedArgs.OldFile}\nNew File: {parsedArgs.NewFile}"); */
            /* Dictionary<string, string> myDict1 = new Dictionary<string, string>(); */
            /* Dictionary<string, string> myDict2 = new Dictionary<string, string>(); */

            /* myDict1.Add("key1","value1"); */
            /* myDict1.Add("key2","value2"); */
            /* myDict2.Add("key1","value3"); */
            /* myDict2.Add("key3","value8"); */
            /* myDict1.Add("key4","value9"); */
            /* myDict2.Add("key5","value8"); */

            /* var keysDictionary1HasThat2DoesNot = myDict1.Keys.Except(myDict2.Keys); */
            /* var keysDictionary2HasThat1DoesNot = myDict2.Keys.Except(myDict1.Keys); */

            /* Console.WriteLine("Exclusive to Dictionary1: "); */
            /* foreach (var key in keysDictionary1HasThat2DoesNot) */
            /* { */
            /*     Console.WriteLine(key.ToString()); */
            /* } */

            /* Console.WriteLine("Exclusive to Dictionary2: "); */
            /* foreach (var key in keysDictionary2HasThat1DoesNot) */
            /* { */
            /*     Console.WriteLine(key.ToString()); */
            /* } */


            /* Console.WriteLine($"File1: {files[0]}, File2: {files[1]}, PrimaryKey: {pKey}"); */

            // Build csvDicts

            // Get Exclusive keys for each file (baseFile and deltaFile)

            // Find keys in both
            // Should be something like
            // IEnumerable<string> both = myDict1.Keys.Intersect(myDict2.Keys)

            // Check for value differences in 'both'
    }
}

using System;
using System.Collections.Generic;
using CsvHelper;

namespace csvd 
{
    public class csvd
    {
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

        static void Main(string[] args)
        {
            /// Parse args - file1 file2 key
            /* string[] files = { args[0], args[1] }; */
            /* int pKey = Int32.Parse(args[2]); */

            Dictionary<string, string> myDict1 = new Dictionary<string, string>();
            Dictionary<string, string> myDict2 = new Dictionary<string, string>();

            myDict1.Add("key1","value1");
            myDict1.Add("key2","value2");
            myDict2.Add("key1","value3");
            myDict2.Add("key3","value8");
            myDict1.Add("key4","value9");
            myDict2.Add("key5","value8");

            var keysDictionary1HasThat2DoesNot = myDict1.Keys.Except(myDict2.Keys);
            var keysDictionary2HasThat1DoesNot = myDict2.Keys.Except(myDict1.Keys);

            Console.WriteLine("Exclusive to Dictionary1: ");
            foreach (var key in keysDictionary1HasThat2DoesNot)
            {
                Console.WriteLine(key.ToString());
            }

            Console.WriteLine("Exclusive to Dictionary2: ");
            foreach (var key in keysDictionary2HasThat1DoesNot)
            {
                Console.WriteLine(key.ToString());
            }


            /* Console.WriteLine($"File1: {files[0]}, File2: {files[1]}, PrimaryKey: {pKey}"); */

            // Build csvDicts

            // Get Exclusive keys for each file (baseFile and deltaFile)

            // Find keys in both
            // Should be something like
            // IEnumerable<string> both = myDict1.Keys.Intersect(myDict2.Keys)

            // Check for value differences in 'both'
        }
    }
}

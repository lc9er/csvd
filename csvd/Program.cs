using System;
using CsvHelper;

namespace csvd 
{
    public class csvd
    {
        static void Main(string[] args)
        {
            /// Parse args - file1 file2 key
            string file1 = args[0];
            string file2 = args[1];
            int pKey = Int32.Parse(args[2]);

            Console.WriteLine($"File1: {file1}, File2: {file2}, PrimaryKey: {pKey}");
        }
    }
}

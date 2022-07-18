using System;
using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;
using CsvHelper;

namespace csvd 
{
    public class csvd
    {
        static void Main(string[] args)
        {
            var parserResults = Parser.Default.ParseArguments<Options>(args);
            parserResults.WithParsed<Options>( opts => 
                    {
                        Console.WriteLine($"OldFile: {opts.OldFile}");
                        Console.WriteLine($"NewFile: {opts.NewFile}");
                        if (opts.pKey.Any() == true)
                        {
                            Console.WriteLine("Primary Key(s): ");
                            foreach (var key in opts.pKey)
                                Console.Write($"{key} ");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Primary Key: 0 (Default Value)");
                        }
                    });
        }
    }
}

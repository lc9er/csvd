using System.CommandLine;
using csvd.UI.Options;


var rootCommand = Options.BuildRootCommand();
ParseResult parseResult = rootCommand.Parse(args);
parseResult.Invoke();
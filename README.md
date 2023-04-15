# csvd
csv diffing tool

# USAGE

```
csvd OldCsvFile.csv NewCsvFile.csv -p 0 -e 1 3
```

diff two csv files, using a common primary key(s). The exclude parameter lets you remove one or more
fields from the comparison.

# JUSTIFICATION

I regularly work with versioned csv files, where there are changes to fields I can safely ignore
(typically datetime values). I also wanted to a chance to try out Spectre.Console.

# INSPIRATION

[xsv](https://github.com/BurntSushi/xsv) - BurntSushi's incredible xsv utility is crazy fast and
offers a huge array of functionality.  
[csvdiff](https://github.com/aswinkarthik/csvdiff) - csvdiff tool in Go. Faster than mine. 

# PLANS

Add binary Powershell cmdlets for use in scripting.

using  System.Collections.Generic;
namespace csvd.Library.Model;

public record FileName(string Filename);
public record Delimiter(char DelimChar);
public class PrimaryKey
{
    public int[] PKey;

    public PrimaryKey() { }
    public PrimaryKey(IEnumerable<int> pKey)
    {
        PKey = [.. pKey];
    }
}

public class ExcludeFields
{
    public int[] Exclude;

    public ExcludeFields() { }
    public ExcludeFields(IEnumerable<int> exclude)
    {
        Exclude = exclude.ToArray();
    }
}

public class HeaderRow
{
    public List<string> Header = new();

    public HeaderRow() { }
    public HeaderRow(List<string> header)
    {
        Header = header;
    }
}

public class CsvDict
{
    public Dictionary<string, IEnumerable<string>> csvDict = new(StringComparer.Ordinal);
}

public class ModifiedCsvDict(string pKey, IEnumerable<string> oldRow, IEnumerable<string> newRow)
{
    public string PrimaryKey          = pKey;
    public IEnumerable<string> OldRow = oldRow;
    public IEnumerable<string> NewRow = newRow;
}

public class CsvFile(string FileName, char DelimChar, IEnumerable<int> PrimaryKey, IEnumerable<int> ExcludeFields)
{
    public FileName fileName           = new(FileName);
    public Delimiter delimiter         = new(DelimChar);
    public PrimaryKey primaryKey       = new(PrimaryKey);
    public ExcludeFields excludeFields = new(ExcludeFields);
    public CsvDict csvFileDict         = new();
    public HeaderRow header            = new();
}

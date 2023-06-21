using System.Collections.Immutable;

namespace csvd.Library.Model;

public record FileName(string Filename);
public record Delimiter(char DelimChar);
public class PrimaryKey
{
    public List<int> PKey = new();

    public PrimaryKey() { }
    public PrimaryKey(List<int> pKey)
    {
        PKey = pKey;
    }
}

public class ExcludeFields
{
    public List<int> Exclude = new();

    public ExcludeFields() { }
    public ExcludeFields(List<int> exclude)
    {
        Exclude = exclude;
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
    public Dictionary<string, IEnumerable<string>> csvDict = new();

    public CsvDict() { }
}

public class CsvFile
{
    public FileName      fileName;
    public Delimiter     delimiter;
    public PrimaryKey    primaryKey;
    public ExcludeFields excludeFields;
    public CsvDict       csvFileDict;
    public HeaderRow     header;

    public CsvFile(string FileName, char DelimChar, List<int> PrimaryKey, List<int> ExcludeFields)
    {
        fileName      = new FileName(FileName);
        delimiter     = new Delimiter(DelimChar);
        primaryKey    = new PrimaryKey(PrimaryKey);
        excludeFields = new ExcludeFields(ExcludeFields);
        csvFileDict   = new CsvDict();
        header        = new HeaderRow();
    }
}

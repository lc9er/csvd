namespace csvd.Library.Model;

public record FileName(string Filename);
public record Delimiter(char DelimChar);
public class PrimaryKey
{
    public int[] PKey;

    public PrimaryKey() { }
    public PrimaryKey(IEnumerable<int> pKey)
    {
        PKey = pKey.ToArray();
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

    public CsvFile(string FileName, char DelimChar, IEnumerable<int> PrimaryKey, IEnumerable<int> ExcludeFields)
    {
        fileName      = new FileName(FileName);
        delimiter     = new Delimiter(DelimChar);
        primaryKey    = new PrimaryKey(PrimaryKey);
        excludeFields = new ExcludeFields(ExcludeFields);
        csvFileDict   = new CsvDict();
        header        = new HeaderRow();
    }
}

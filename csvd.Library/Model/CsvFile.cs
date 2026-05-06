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
    public Dictionary<string, IEnumerable<string>> csvDict = new(StringComparer.Ordinal);

    public CsvDict() { }

    public (HashSet<string> oldUniqKeys, HashSet<string> modifiedKeys, HashSet<string> newUniqKeys) CompareTo (CsvDict comparisonObj)
    {
        HashSet<string> oldKeys      = [];
        HashSet<string> modifiedKeys = [];
        HashSet<string> newKeys      = [];

        // Populate oldKeys to use as reference
        foreach (var key in this.csvDict.Keys)
            oldKeys.Add(key);

        // Compare against new
        foreach (var key in comparisonObj.csvDict.Keys)
        {
            // If match found, check for changes 
            // Else, add to newKeys
            if (oldKeys.Remove(key))
            {
                if (!this.csvDict[key].SequenceEqual(comparisonObj.csvDict[key], StringComparer.Ordinal))
                {
                    modifiedKeys.Add(key);
                }
            }
            else
            {
                newKeys.Add(key);
            }
        }

        return (oldKeys, modifiedKeys, newKeys);
    }
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

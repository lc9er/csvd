namespace csvd.Library.Model;

public class CsvFile
{
    public string fileName;
    public char delimiter;
    public List<int> primaryKey;
    public List<int> excludeFields;
    public Dictionary<string, List<string>> csvFileDict = new ();
    public List<string> header = new ();

    public CsvFile(string FileName, char DelimChar, List<int> PrimaryKey, List<int> ExcludeFields)
    {
        fileName      = FileName;
        delimiter     = DelimChar;
        primaryKey    = PrimaryKey;
        excludeFields = ExcludeFields;
    }
}

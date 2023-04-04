using csvd.Library.Model;

namespace csvd.Library.Interfaces;

public interface IDataAccess
{
    Dictionary<string, List<string>> GetData(CsvFile csvFile);
}

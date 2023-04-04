using csvd.Domain.Model;

namespace csvd.Data.csvd.Data;

public interface IDataAccess
{
    Dictionary<string, List<string>> GetData(CsvFile csvFile);
}

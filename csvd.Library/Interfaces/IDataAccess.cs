using csvd.Library.Model;

namespace csvd.Library.Interfaces;

public interface IDataAccess
{
    CsvDict GetData(CsvFile csvFile);
}

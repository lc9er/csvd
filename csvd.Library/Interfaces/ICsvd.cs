using csvd.Library.Model;

namespace csvd.Library.Interfaces;

public interface ICsvd
{
    IEnumerable<string> GetModifiedKeys(IEnumerable<string> sharedKeys,
        CsvDict oldFileDict,
        CsvDict newFileDict);

    IEnumerable<string> GetUniqueKeys(IEnumerable<string> oldKeys, IEnumerable<string> newKeys);
    IEnumerable<string> GetSharedKeys(IEnumerable<string> oldKeys, IEnumerable<string> newKeys);
}

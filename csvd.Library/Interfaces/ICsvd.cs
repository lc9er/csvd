using csvd.Library.Model;

namespace csvd.Library.Interfaces;

public interface ICsvd
{
    IEnumerable<ulong> GetModifiedKeys(IEnumerable<ulong> sharedKeys,
        CsvDict oldFileDict,
        CsvDict newFileDict);

    IEnumerable<ulong> GetUniqueKeys(IEnumerable<ulong> oldKeys, IEnumerable<ulong> newKeys);
    IEnumerable<ulong> GetSharedKeys(IEnumerable<ulong> oldKeys, IEnumerable<ulong> newKeys);
}

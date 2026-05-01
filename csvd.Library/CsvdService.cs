using System.Collections.Generic;
using csvd.Library.Interfaces;
using csvd.Library.Model;

namespace csvd.Library;

public class CsvdService : ICsvd
{
    public IEnumerable<ulong> GetModifiedKeys(IEnumerable<ulong> sharedKeys, CsvDict oldFileDict, CsvDict newFileDict) =>
        sharedKeys.Where(x => GetModifiedValues(x, oldFileDict, newFileDict));

    private static bool GetModifiedValues(ulong key, CsvDict oldDict, CsvDict newDict) =>
        !oldDict.csvDict[key].SequenceEqual(newDict.csvDict[key]);

    public IEnumerable<ulong> GetUniqueKeys(IEnumerable<ulong> oldKeys, IEnumerable<ulong> newKeys) => 
        oldKeys.Except(newKeys);

    public IEnumerable<ulong> GetSharedKeys(IEnumerable<ulong> oldKeys, IEnumerable<ulong> newKeys) =>
        oldKeys.Intersect(newKeys);
}

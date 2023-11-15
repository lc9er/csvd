using System.Collections.Generic;
using csvd.Library.Interfaces;
using csvd.Library.Model;

namespace csvd.Library;

public class CsvdService : ICsvd
{
    public IEnumerable<string> GetModifiedKeys(IEnumerable<string> sharedKeys, CsvDict oldFileDict, CsvDict newFileDict) =>
        sharedKeys.Where(x => GetModifiedValues(x, oldFileDict, newFileDict));

    private static bool GetModifiedValues(string key, CsvDict oldDict, CsvDict newDict) =>
        !oldDict.csvDict[key].SequenceEqual(newDict.csvDict[key], StringComparer.Ordinal);

    public IEnumerable<string> GetUniqueKeys(IEnumerable<string> oldKeys, IEnumerable<string> newKeys) => 
        oldKeys.Except(newKeys, StringComparer.Ordinal);

    public IEnumerable<string> GetSharedKeys(IEnumerable<string> oldKeys, IEnumerable<string> newKeys) =>
        oldKeys.Intersect(newKeys, StringComparer.Ordinal);
}

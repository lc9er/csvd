using csvd.Library.Interfaces;

namespace csvd.Library;

public class CsvdService : ICsvd
{
    public List<string> GetModifiedKeys(List<string> sharedKeys, Dictionary<string, List<string>> oldFileDict, Dictionary<string, List<string>> newFileDict) =>
        sharedKeys.Where(x => GetModifiedValues(x, oldFileDict, newFileDict)).ToList();

    private static bool GetModifiedValues(string key, Dictionary<string, List<string>> oldDict, Dictionary<string, List<string>> newDict) =>
        !oldDict[key].SequenceEqual(newDict[key]);

    public List<string> GetUniqueKeys(List<string> oldKeys, List<string> newKeys) => 
        oldKeys.Except(newKeys).ToList();

    public List<string> GetSharedKeys(List<string> oldKeys, List<string> newKeys) => 
        oldKeys.Intersect(newKeys).ToList();
}

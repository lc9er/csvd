namespace csvd.Library.Interfaces;

public interface ICsvd
{
    List<string> GetModifiedKeys(List<string> sharedKeys,
        Dictionary<string, List<string>> oldFileDict,
        Dictionary<string, List<string>> newFileDict);

    List<string> GetUniqueKeys(List<string> oldKeys, List<string> newKeys);
    List<string> GetSharedKeys(List<string> oldKeys, List<string> newKeys);
}

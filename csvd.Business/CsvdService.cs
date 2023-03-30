﻿using csvd.Business.Interfaces;

namespace csvd.Business;

public class CsvdService : ICsvd
{
    public List<string> GetModifiedKeys(List<string> sharedKeys, Dictionary<string, List<string>> oldFileDict, Dictionary<string, List<string>> newFileDict)
    {
        List<string> modifiedKeys = new ();

        foreach (var key in sharedKeys)
        {
            List<string> oldVals = oldFileDict[key];
            List<string> newVals = newFileDict[key];

            if (!(oldVals.SequenceEqual(newVals)))
                modifiedKeys.Add(key);
        }

        return modifiedKeys;
    }

    public List<string> GetUniqueKeys(List<string> oldKeys, List<string> newKeys)
    {
        return oldKeys.Except(newKeys).ToList();
    }

    public List<string> GetSharedKeys(List<string> oldKeys, List<string> newKeys)
    {
        return oldKeys.Intersect(newKeys).ToList();
    }
}
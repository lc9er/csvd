namespace csvd
{
    public class ParseArgs
    {
        public string OldFile;
        public string NewFile;
        public int[] PrimaryKey;
        public int[] ExcludeFields;

        public int[] GetKeyVals(string[] cliArgs, string key)
        {
            // if primary key index not specified, return {0}
            if (key == "-p" && !cliArgs.Contains(key))
            {
                return new int[] { 0 };
            }
            else if (key == "-e" && !cliArgs.Contains(key))
            {
                return new int[] {};
            }
            else
            {
                // index of -p param
                int keyIndex = Array.IndexOf(cliArgs, key);

                // -p provided
                if (keyIndex + 1 < cliArgs.Length)
                {
                    string[] keyVals = cliArgs[keyIndex + 1].Split(',');
                    int[] pKeyArray = new int[keyVals.Length];

                    for (var i = 0; i < keyVals.Length; i++)
                    {
                        // Only take ints as key(s)
                        if (Int32.TryParse(keyVals[i], out int pKeyInt))
                        {
                            pKeyArray[i] = pKeyInt;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format($"{key} values must be an integer"));
                        }
                    }

                    return pKeyArray;
                }
                // -p flag, but no key listed
                else 
                {
                    throw new ArgumentException(String.Format($"Missing {key} values"));
                }
            }
        }

        public ParseArgs(string[] cliArgs)
        {
            if (cliArgs.Length < 2)
            {
                throw new ArgumentException(String.Format("Insufficient args"));
            }
            else
            {
                OldFile = cliArgs[0];
                NewFile = cliArgs[1];
            }

            PrimaryKey = GetKeyVals(cliArgs, "-p");
            ExcludeFields = GetKeyVals(cliArgs,"-e");
        }
    }
}

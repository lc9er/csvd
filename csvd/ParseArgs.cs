namespace csvd
{
    public class ParseArgs
    {
        public string OldFile;
        public string NewFile;
        public int[] PrimaryKey;

        public int[] GetPKey(string[] cliArgs)
        {
            if (!cliArgs.Contains("-p"))
            {
                return new int[] { 0 };
            }
            else
            {
                // index of -p param
                int pIndex = Array.IndexOf(cliArgs, "-p");

                // -p provided
                if (pIndex + 1 < cliArgs.Length)
                {
                    string[] pKey = cliArgs[pIndex + 1].Split(',');
                    int[] pKeyArray = new int[pKey.Length];

                    for (var i = 0; i < pKey.Length; i++)
                    {
                        // Only take ints as key(s)
                        if (Int32.TryParse(pKey[i], out int pKeyInt))
                        {
                            pKeyArray[i] = pKeyInt;
                        }
                        else
                        {
                            throw new ArgumentException(String.Format("Primary key must be an integer"));
                        }
                    }

                    return pKeyArray;
                }
                // -p flag, but no key listed
                else 
                {
                    throw new ArgumentException(String.Format("Missing primary key"));
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

            PrimaryKey = GetPKey(cliArgs);
        }
    }
}

namespace csvd.tests;

public class UnitTest1
{
    [Theory]
    [InlineData("File1","File2")]
    [InlineData("oldfile","newfile")]
    public void ParseArgs_ReturnsFileNames(params string[] value)
    {
        var ParsedArgs = new ParseArgs(value);

        Assert.Equal(value[0], ParsedArgs.OldFile);
        Assert.Equal(value[1], ParsedArgs.NewFile);
    }

    [Fact]
    public void ParseArgs_ReturnsException()
    {
        var testArgs = new string[]{};
        ArgumentException ex = Assert.Throws<ArgumentException>(() => new ParseArgs(testArgs));

        Assert.Equal("Insufficient args", ex.Message);
    }

    [Fact]
    public void ParseArgs_ReturnPKeyEmpty()
    {
        string[] testArgs = {"File1", "File2"};
        int[] pKeyZero = {0};
        var ParsedArgs = new ParseArgs(testArgs);
        Assert.Equal(pKeyZero, ParsedArgs.PrimaryKey);
    }

    [Fact]
    public void ParseArgs_ReturnPKeyValue()
    {
        string[] testArgs = {"File1", "File2", "-p", "1"};
        int[] pKeyOne = {1};
        var ParsedArgs = new ParseArgs(testArgs);
        Assert.Equal(pKeyOne, ParsedArgs.PrimaryKey);
    }

    [Fact]
    public void ParseArgs_ReturnPKey_MissingPKey()
    {
        string[] testArgs = {"File1", "File2", "-p"};
        ArgumentException ex = Assert.Throws<ArgumentException>(() => new ParseArgs(testArgs));
        Assert.Equal("Missing -p values", ex.Message);
    }

    [Fact]
    public void ParseArgs_ReturnPKey_Multiple()
    {
        string[] testArgs = {"File1", "File2","-p","0,1"};
        int[] pKeys = {0,1};
        var ParsedArgs = new ParseArgs(testArgs);
        Assert.Equal(pKeys, ParsedArgs.PrimaryKey);
    }

    [Fact]
    public void ParseArgs_ReturnPKey_StringNotInt()
    {
        string[] testArgs = {"File1", "File2", "-p", "BADKEY"};
        ArgumentException ex = Assert.Throws<ArgumentException>(() => new ParseArgs(testArgs));
        Assert.Equal("-p values must be an integer", ex.Message);
    }

    [Fact]
    public void ParseArgs_Return_ExcludeFields()
    {
        string[] testArgs = {"File1", "File2", "-e", "3,4"};
        int[] excludes = {3,4};
        var ParsedArgs = new ParseArgs(testArgs);
        Assert.Equal(excludes, ParsedArgs.ExcludeFields);
    }

    [Fact]
    public void ParseArgs_Return_EmptyExcludeFields()
    {
        string[] testArgs = {"File1", "File2"};
        int[] excludes = {};
        var ParsedArgs = new ParseArgs(testArgs);
        Assert.Equal(excludes, ParsedArgs.ExcludeFields);
    }
}


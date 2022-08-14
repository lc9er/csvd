namespace csvd.tests;

public class UnitTest1
{
    /* [Theory] */
    /* [InlineData("File1","File2")] */
    /* [InlineData("oldfile","newfile")] */
    /* public void ParseArgs_ReturnsFileNames(params string[] value) */
    /* { */
    /*     var ParsedArgs = new ParseArgs(value); */

    /*     Assert.Equal(value[0], ParsedArgs.OldFile); */
    /*     Assert.Equal(value[1], ParsedArgs.NewFile); */
    /* } */

    [Fact]
    public void ParseCsv_Contructor()
    {
        string testFileName = "TestFile.csv";
        ParseCsv myCsvObj = new ParseCsv(testFileName);

        Assert.Equal(testFileName, myCsvObj.fileName);
    }
}

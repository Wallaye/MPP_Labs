namespace TestGenerator.Core.Data;

public class TestFile
{
    public string FileName { get; }
    public string Content { get; }

    public TestFile(string fileName, string content)
    {
        FileName = fileName;
        Content = content;
    }
}
namespace TestGenerator.Core.Generator;

public class GeneratorOptions
{
    public int MaxThreadsToRead { get; set; }
    public int MaxThreadsToWrite { get; set; }
    public int MaxThreadsToGenerate { get; set; }
    public string SourceDir { get; set; }
    public string DestDir { get; set; }

    public GeneratorOptions(int r, int w, int g, string src, string dest)
    {
        MaxThreadsToRead = r;
        MaxThreadsToWrite = w;
        MaxThreadsToGenerate = g;
        SourceDir = src;
        DestDir = dest;
    }
}
namespace AssemblyAnalyzer.Data;

public class NamespaceData
{
    public string? Name { get; private set; }
    public List<ClassData> Classes { get; private set; }

    public NamespaceData(string? name)
    {
        Name = name;
        Classes = new();
    }
}
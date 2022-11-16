using System.Reflection;

namespace AssemblyAnalyzer.Data;

public class AssemblyData
{
    public Assembly Asm{ get; private set; }
    public List<NamespaceData> Namespaces { get; private set; }

    public AssemblyData(Assembly asm)
    {
        Asm = asm;
        Namespaces = new();
    }
}
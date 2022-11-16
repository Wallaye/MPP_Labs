using System.Reflection;

namespace AssemblyAnalyzer.Data;

public class AssemblyData
{
    public Assembly Asm{ get; private set; }
    public HashSet<NamespaceData> Namespaces { get; private set; }

    public AssemblyData(Assembly asm)
    {
        Asm = asm;
        Namespaces = new HashSet<NamespaceData>();
    }
}
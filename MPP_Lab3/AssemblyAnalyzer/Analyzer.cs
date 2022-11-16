using System.Reflection;
using System.Runtime.CompilerServices;
using AssemblyAnalyzer.Data;

namespace AssemblyAnalyzer;

public class Analyzer
{
    public static AssemblyData Analyze(Assembly asm)
    {
        AssemblyData result = new AssemblyData(asm);
        GetNamespaces(result);
        

        return result;
    }

    private static void AddNamespace(Type type, AssemblyData asm)
    {
        var list = asm.Namespaces;
        var name = type.Namespace;
        if (name == null) return;
        list.Add(new(name));
    }

    private static void GetNamespaces(AssemblyData asm)
    {
        foreach (var type in asm.Asm.GetTypes())
        {
            AddNamespace(type, asm);
        }
    }
}
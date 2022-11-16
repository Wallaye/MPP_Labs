using System.Reflection;
using AssemblyAnalyzer.Data;

namespace AssemblyAnalyzer;

public class Analyzer
{
    public static AssemblyData Analyze(Assembly asm)
    {
        AssemblyData result = new AssemblyData(asm);
        foreach (var type in asm.GetTypes())
        {
            ClassData classData = new(type);
            
        }

        return result;
    }
}
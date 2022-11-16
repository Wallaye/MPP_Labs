using System.Reflection;

namespace AssemblyAnalyzer.Data;

public class MethodData
{
    public MethodInfo Info { get; private set; }
    public bool IsExtension { get; private set; }

    public MethodData(MethodInfo info, bool isExtension)
    {
        Info = info;
        IsExtension = isExtension;
    }
}
using System.Reflection;

namespace AssemblyAnalyzer.Data;

public class ClassData
{
    public Type ClassType { get; private set; }
    public List<MethodInfo> Methods { get; private set; }
    public List<PropertyInfo> Properties { get; private set; }
    public List<FieldInfo> Fields { get; private set; }
    public List<ConstructorInfo> Constructor { get; private set; }
    public ClassData(Type t) => this.ClassType = t;
}
using System.Reflection;

namespace AssemblyAnalyzer.Data;

public class ClassData
{
    public Type ClassType { get; private set; }
    public List<MethodData> Methods { get; private set; }
    public List<PropertyInfo> Properties { get; private set; }
    public List<FieldInfo> Fields { get; private set; }
    public List<ConstructorInfo> Constructors { get; private set; }

    public ClassData(Type t)
    {
        this.ClassType = t;
        Methods = new();
        Properties = new();
        Fields = new();
        Constructors = new();
    }
}
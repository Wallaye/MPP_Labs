namespace Tracer.Core;

public class MethodInfo
{
    public string Name { get; }
    public string ClassName { get; }
    public long Time { get; private set; }
    private List<MethodInfo> _methods;

    public IReadOnlyList<MethodInfo> Methods
    {
        get => _methods;
    }

    public MethodInfo(string name = "", string className = "", long time = 0)
    {
        Name = name;
        ClassName = className;
        Time = time;
        _methods = new List<MethodInfo>();
    }

    internal void SetTime(long time)
    {
        Time = time;
    }

    internal void AddMethod(MethodInfo method)
    { 
        _methods.Add(method);
    }
}
using System.Text.Json.Serialization;
using System.Xml.Serialization;
namespace Tracer.Core;

[Serializable]
public class MethodInfo
{
    [XmlAttribute("name")]
    [JsonPropertyName("name")]
    public string Name { get; }
    [XmlAttribute("classname")]
    [JsonPropertyName("classname")]
    public string ClassName { get; }
    [XmlAttribute("time")]
    [JsonPropertyName("time")]
    public long Time { get; private set; }
    [XmlAttribute("methods")]
    [JsonPropertyName("methods")]
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
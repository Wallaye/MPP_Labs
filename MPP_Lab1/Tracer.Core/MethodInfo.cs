using System.Text.Json.Serialization;
using System.Xml.Serialization;
namespace Tracer.Core;

[Serializable]
[XmlType(TypeName = "method")]
public class MethodInfo
{
    [XmlAttribute(AttributeName = "name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }
    internal long Time { get; set; }
    [XmlAttribute(AttributeName = "time")]
    [JsonPropertyName("time")]
    public string TimeString
    {
        get { return (Time.ToString() + "ms"); }
        set { }
    }
    [XmlAttribute(AttributeName = "classname")]
    [JsonPropertyName("classname")]
    public string ClassName { get; set; }
    [XmlElement(ElementName = "method")]
    [JsonPropertyName("methods")]
    public List<MethodInfo> methods { get; set; }

    public MethodInfo(string name = "", string className = "", long time = 0)
    {
        Name = name;
        ClassName = className;
        Time = time;
        methods = new List<MethodInfo>();
    }

    private MethodInfo() { }

    internal void SetTime(long time)
    {
        Time = time;
    }

    internal void AddMethod(MethodInfo method)
    { 
        methods.Add(method);
    }
}
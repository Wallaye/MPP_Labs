using System.Text.Json.Serialization;
using System.Xml.Serialization;
namespace Tracer.Core;

[Serializable]
[XmlType(TypeName = "thread")]
public class ThreadInfo
{
    [XmlAttribute(AttributeName = "id")]
    [JsonPropertyName("id")]
    public int ThreadId { get; set; }
    [XmlAttribute(AttributeName = "time")]
    [JsonPropertyName("time")]
    public string TimeString
    {
        get => Time.ToString() + "ms";
        set { }
    }
    private long Time { get; set; }
    [XmlElement(ElementName = "method")]
    [JsonPropertyName("methods")]
    public List<MethodInfo> Methods { set; get; }
    
    public ThreadInfo(int id, long time, List<MethodInfo> list)
    {
        ThreadId = id;
        Time = time;
        Methods = list;
    }
    private ThreadInfo() { }
}
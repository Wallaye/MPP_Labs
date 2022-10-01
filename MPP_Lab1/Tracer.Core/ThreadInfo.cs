using System.Text.Json.Serialization;
using System.Xml.Serialization;
namespace Tracer.Core;

public class ThreadInfo
{
    [XmlAttribute("id")]
    [JsonPropertyName("id")]
    public int ThreadId { get; set; }
    [XmlAttribute("time")]
    [JsonPropertyName("time")]
    public long Time { get; set; }
    [XmlAttribute("methods")]
    [JsonPropertyName("methods")]
    public IReadOnlyList<MethodInfo> Methods { get; }
    
    public ThreadInfo(int id, long time, List<MethodInfo> list)
    {
        ThreadId = id;
        Time = time;
        Methods = list;
    }
}
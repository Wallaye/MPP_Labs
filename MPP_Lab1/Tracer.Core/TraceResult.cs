using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Tracer.Core;

[Serializable]
[XmlType(TypeName = "root")]
public class TraceResult
{
    [JsonPropertyName("threads")]
    [XmlElement(ElementName = "thread")]
    public List<ThreadInfo> Threads { set; get; }

    public TraceResult(List<ThreadInfo> list)
    {
        Threads = list;
    }
    private TraceResult() { }
}
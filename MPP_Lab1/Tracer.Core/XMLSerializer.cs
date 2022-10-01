using System.Xml.Serialization;

namespace Tracer.Core
{
    public class XMLSerializer : ITraceSerializer
    {
        public void Serialize(TraceResult value, Stream stream)
        {
            using (stream)
            {
                XmlSerializer ser = new(typeof(TraceResult));
                ser.Serialize(stream, value);
            }
        }
    }
}

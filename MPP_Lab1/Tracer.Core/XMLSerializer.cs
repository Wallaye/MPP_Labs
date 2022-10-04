using System.Xml.Serialization;
using System.Xml;

namespace Tracer.Core
{
    public class XMLSerializer : ITraceSerializer
    {
        public void Serialize(TraceResult value, Stream stream)
        {
            var xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            using (var xmlWriter = XmlWriter.Create(stream, xmlSettings))
            {
                XmlSerializer ser = new(typeof(TraceResult),
                    new Type[] { typeof(MethodInfo), typeof(ThreadInfo)});
                ser.Serialize(xmlWriter, value);
            }
        }
    }
}

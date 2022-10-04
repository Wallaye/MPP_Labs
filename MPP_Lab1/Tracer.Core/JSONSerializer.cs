using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Tracer.Core
{
    public class JSONSerializer : ITraceSerializer
    {
        public void Serialize(TraceResult value, Stream stream)
        {
            using (stream)
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var result = JsonSerializer.Serialize(value, options);
                stream.Write(Encoding.UTF8.GetBytes(result));
            }
        }
    }
}

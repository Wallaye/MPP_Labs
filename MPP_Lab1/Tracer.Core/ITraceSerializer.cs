using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Core
{
    internal interface ITraceSerializer
    {
        public void Serialize(TraceResult value, Stream stream);
    }
}

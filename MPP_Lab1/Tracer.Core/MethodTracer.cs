using System.Diagnostics;

namespace Tracer.Core;

public class MethodTracer : ITracer<long>
{
    private Stopwatch _sw = new Stopwatch();
    public void StartTrace() => _sw.Start();
    public void StopTrace() => _sw.Stop();
    public long GetTraceResult() => _sw.ElapsedMilliseconds;
}

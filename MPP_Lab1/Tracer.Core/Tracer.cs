namespace Tracer.Core;

public class Tracer : ITracer<TraceResult>
{
    public void StartTrace()
    {
        
    }

    public void StopTrace()
    {
        
    }

    public TraceResult GetTraceResult()
    {
        return new TraceResult(new List<ThreadInfo>());
    }
}

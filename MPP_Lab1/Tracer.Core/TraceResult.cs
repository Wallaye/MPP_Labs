namespace Tracer.Core;

public class TraceResult
{
    public IReadOnlyList<ThreadInfo> Threads { get; }

    public TraceResult(List<ThreadInfo> list)
    {
        Threads = list;
    }
    private TraceResult()
    {

    }
}
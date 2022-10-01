namespace Tracer.Core;

public class ThreadInfo
{
    public int ThreadId { get; set; }
    public long Time { get; set; }
    public IReadOnlyList<MethodInfo> Methods { get; }
    
    public ThreadInfo(int id, long time, List<MethodInfo> list)
    {
        ThreadId = id;
        Time = time;
        Methods = list;
    }
}
using System.Diagnostics;

namespace Tracer.Core;

public class ThreadTracer : ITracer<ThreadInfo>
{
    private class MethodRes
    {
        public MethodInfo info { get; set; }
        public MethodTracer tracer { get; set; }

        public MethodRes(MethodInfo info)
        {
            this.info = info;
            tracer = new MethodTracer();
        }
    }

    private readonly Stack<MethodRes> _stack = new Stack<MethodRes>();
    private readonly List<MethodInfo> _rootMethods = new List<MethodInfo>();
    private readonly int _frameNumber;
    private readonly int _threadId = Environment.CurrentManagedThreadId;

    public ThreadTracer(int frameNumber = 3) => _frameNumber = frameNumber;
    
    public void StartTrace()
    {
        MethodInfo info = GetMethodInfo();
        MethodRes methodRes = new(info);
        _stack.Push(methodRes);
        methodRes.tracer.StartTrace();
    }

    public void StopTrace()
    {
        if (!_stack.TryPop(out MethodRes? child))
        {
            MethodInfo info = child!.info;
            child.info.SetTime(child.tracer.GetTraceResult());
            if (_stack.TryPeek(out MethodRes? parent))
            {
                parent.info.AddMethod(child.info);
            }
            else
            {
                _rootMethods.Add(child.info);
            }
        }
    }
    
    public ThreadInfo GetTraceResult()
    {
        long time = 0;
        foreach (var method in _rootMethods)
        {
            time += method.Time;
        }

        return new ThreadInfo(_threadId, time, _rootMethods);
    }

    private MethodInfo GetMethodInfo()
    {
        StackTrace stackTrace = new();
        StackFrame? stackFrame = stackTrace.GetFrame(_frameNumber);
        
        if (stackFrame != null)
        {
            var method = stackFrame.GetMethod();
            return new MethodInfo(method!.Name, method!.DeclaringType!.Name);
        }

        return new MethodInfo();
    }
}
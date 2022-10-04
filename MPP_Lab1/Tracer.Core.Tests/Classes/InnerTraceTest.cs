namespace Tracer.Core.Tests.Classes
{
    internal class InnerTraceTest
    {
        private ITracer<TraceResult> _tracer;
        public InnerTraceTest(ITracer<TraceResult> tracer)
        {
            _tracer = tracer;
        }   

        public void InnerMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(300);
            _tracer.StopTrace();
        }
    }
}

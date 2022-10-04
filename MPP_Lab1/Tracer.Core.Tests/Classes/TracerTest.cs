namespace Tracer.Core.Tests.Classes
{
    internal class TracerTest
    {
        private InnerTraceTest _inner;
        private ITracer<TraceResult> _tracer;

        internal TracerTest(ITracer<TraceResult> tracer)
        {
            _tracer = tracer;
            _inner = new InnerTraceTest(_tracer);
        }

        public void PublicMethod()
        {
            _tracer.StartTrace();
            _inner.InnerMethod();
            Thread.Sleep(100);
            PrivateMethod();
            _tracer.StopTrace();
        }

        private void PrivateMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(200);
            _tracer.StopTrace();
        }
    }
}

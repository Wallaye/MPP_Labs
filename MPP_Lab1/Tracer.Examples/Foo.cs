using Tracer.Core;

namespace Tracer.Examples
{
    public class Foo
    {
        private Bar _bar;
        private ITracer<TraceResult> _tracer;

        internal Foo(ITracer<TraceResult> tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }

        public void PublicMethod()
        {
            _tracer.StartTrace();
            _bar.InnerMethod();
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


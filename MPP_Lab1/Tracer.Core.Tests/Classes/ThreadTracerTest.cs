namespace Tracer.Core.Tests.Classes
{
    internal class ThreadTracerTest
    {
        private ITracer<ThreadInfo> _tracer;

        public ThreadTracerTest(ITracer<ThreadInfo> tracer) => _tracer = tracer;

        public void T1()
        {
            _tracer.StartTrace();
            Thread.Sleep(300);
            _tracer.StopTrace();
        }

        public void T2()
        {
            _tracer.StartTrace();
            Thread.Sleep(200);
            T3();
            _tracer.StopTrace();
        }

        public void T3()
        {
            _tracer.StartTrace();
            Thread.Sleep(300);
            _tracer.StopTrace();
        }

        public void T4()
        {
            T2();
            T3();
        }
    }
}

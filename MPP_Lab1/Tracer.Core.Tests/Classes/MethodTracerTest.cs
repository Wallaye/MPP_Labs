namespace Tracer.Core.Tests.Classes
{
    internal class MethodTracerTest
    {
        private ITracer<long> _tracer;
        public MethodTracerTest(ITracer<long> tracer)
        {
            _tracer = tracer;
        }

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

        private void T3()
        {
            Thread.Sleep(300);
        }
    }
}

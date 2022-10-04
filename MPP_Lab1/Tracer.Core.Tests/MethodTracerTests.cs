using Tracer.Core.Tests.Classes;
namespace Tracer.Core.Tests
{
    public class MethodTracerTests
    {
        [Test]
        public void GetTraceResult_Returned300()
        {
            var methodTracer = new MethodTracer();
            var testClass = new MethodTracerTest(methodTracer);
            long expected = 300;
            testClass.T1();
            long actual = methodTracer.GetTraceResult();
            Assert.That(actual, Is.EqualTo(expected).Within(25));
        }

        [Test]
        public void GetTraceResult_WithInnerMethod_Returned500()
        {
            var methodTracer = new MethodTracer();
            var testClass = new MethodTracerTest(methodTracer);
            long expected = 500;
            testClass.T2();
            long actual = methodTracer.GetTraceResult();
            Assert.That(actual, Is.EqualTo(expected).Within(25));
        }
    }
}
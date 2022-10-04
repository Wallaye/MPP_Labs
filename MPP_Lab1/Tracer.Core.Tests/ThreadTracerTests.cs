using Tracer.Core.Tests.Classes;

namespace Tracer.Core.Tests
{
    internal class ThreadTracerTests
    {
        [Test]
        public void SingleMethodCallMethodNameShouldBeT1()
        {
            var threadTracer = new ThreadTracer(2);
            var testClass = new ThreadTracerTest(threadTracer);
            string expected = "T1";
            testClass.T1();
            string actual = threadTracer.GetTraceResult().Methods[0].Name;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void WithInnerMethodMethodTimeShouldBe500()
        {
            var threadTracer = new ThreadTracer(2);
            var testClass = new ThreadTracerTest(threadTracer);
            long expected = 500;
            testClass.T2();
            string actual_string = threadTracer.GetTraceResult().Methods[0].TimeString;
            long actual = Convert.ToInt64(actual_string.Substring(0, actual_string.Length - 2));
            Assert.That(actual, Is.EqualTo(expected).Within(25));
        }


        [Test]
        public void ThreadInfo_ThreadIdCheck_MethodsCountShouldBe2()
        {
            var threadTracer = new ThreadTracer(2);
            var testClass = new ThreadTracerTest(threadTracer);
            int idExpected = Environment.CurrentManagedThreadId;
            int countExpected = 2;
            testClass.T4();
            var result = threadTracer.GetTraceResult();
            Assert.Multiple(() =>
            {
                Assert.That(result.ThreadId, Is.EqualTo(idExpected));
                Assert.That(result.Methods.Count, Is.EqualTo(countExpected));
            });
        }

        [Test]
        public void ThreadInfo_ThreadTimeShouldBeSumOfMethodsTime()
        {
            var threadTracer = new ThreadTracer(2);
            var testClass = new ThreadTracerTest(threadTracer);
            long expected = 800;
            testClass.T4();
            string actual_string = threadTracer.GetTraceResult().TimeString;
            long actual = Convert.ToInt64(actual_string.Substring(0, actual_string.Length - 2));
            Assert.That(actual, Is.EqualTo(expected).Within(50));
        }
    }
}

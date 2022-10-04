using Tracer.Core.Tests.Classes;
namespace Tracer.Core.Tests
{
    internal class TracerTests
    {
        [Test]
        public void SingleThread_TimeShouldBe600()
        {
            var tracer = new Tracer();
            var testClass = new TracerTest(tracer);
            long expectedTime = 600;
            int expectedCount = 1;
            testClass.PublicMethod();
            var res = tracer.GetTraceResult();
            string s = res.Threads[0].TimeString;
            long l = Convert.ToInt64(s.Substring(0, s.Length - 2));
            Assert.Multiple(() =>
            {
                Assert.That(res.Threads.Count, Is.EqualTo(expectedCount));
                Assert.That(l, Is.EqualTo(expectedTime).Within(50));
            });
        }
        [Test]
        public void MultiThread_ThreadIdShouldBeDifferent()
        {
            var tracer = new Tracer();
            var testClass = new TracerTest(tracer);
            var innerClass = new InnerTraceTest(tracer);
            int expectedCount = 2;
            testClass.PublicMethod();
            var task = Task.Run(() => testClass.PublicMethod());
            testClass.PublicMethod();
            innerClass.InnerMethod();
            task.Wait();
            var res = tracer.GetTraceResult();
            Assert.Multiple(() =>
            {
                Assert.That(res.Threads.Count, Is.EqualTo(expectedCount));
                Assert.That(res.Threads[0].ThreadId, Is.Not.EqualTo(res.Threads[1].ThreadId));
            });
        }
    }
}

namespace Faker.Core.Tests
{
    public class TestCtor
    {
        public int a;
        public int b;
        public int C { get; }

        public TestCtor()
        { }

        public TestCtor(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public TestCtor(int a, int b, int c) : this(a, b)
        {
            this.C = c;
        }
    }
}

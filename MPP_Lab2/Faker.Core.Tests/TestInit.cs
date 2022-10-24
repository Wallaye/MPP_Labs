namespace Faker.Core.Tests
{
    public class TestInit
    {
        public int Int { get; set; }
        public string String { get; set; }
        public byte Byte { get; set; }
        public B? b;
        public bool Bool { get; set; }

        public TestInit parent;

        public TestInit(int intValue, string stringValue)
        {
            Int = intValue;
            String = stringValue;
        }
    }

    public class B
    {
        public char symbol;

        private decimal dec;
    }
}

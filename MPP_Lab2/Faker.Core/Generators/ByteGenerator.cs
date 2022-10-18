namespace Faker.Core.Generators
{
    internal class ByteGenerator : IValueGenerator
    {
        public bool CanGenerate(Type Type)
        {
            return Type == typeof(byte);
        }

        public object Generate(Type TypeToGenerate, GeneratorContext context)
        {
            return (byte)context.Random.Next(byte.MinValue, byte.MaxValue);
        }
    }
}

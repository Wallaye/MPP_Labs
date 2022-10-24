namespace Faker.Core.Generators
{
    internal class LongGenerator : IValueGenerator
    {
        public bool CanGenerate(Type Type)
        {
            return Type == typeof(long);
        }

        public object Generate(Type TypeToGenerate, GeneratorContext context)
        {
            return context.Random.NextInt64(1, long.MaxValue);
        }
    }
}

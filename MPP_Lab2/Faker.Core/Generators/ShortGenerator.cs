namespace Faker.Core.Generators
{
    internal class ShortGenerator : IValueGenerator
    {
        public bool CanGenerate(Type Type)
        {
            return Type == typeof(short);
        }

        public object Generate(Type TypeToGenerate, GeneratorContext context)
        {
            return (short)context.Random.Next(1, short.MaxValue);
        }
    }
}

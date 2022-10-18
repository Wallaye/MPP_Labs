namespace Faker.Core.Generators
{
    internal class IntGenerator : IValueGenerator
    {
        public bool CanGenerate(Type Type)
        {
            return Type == typeof(int);
        }

        public object Generate(Type TypeToGenerate, GeneratorContext context)
        {
            return context.Random.Next(int.MinValue, int.MaxValue);
        }
    }
}

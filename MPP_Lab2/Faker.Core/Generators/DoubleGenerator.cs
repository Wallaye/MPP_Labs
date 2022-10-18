namespace Faker.Core.Generators
{
    internal class DoubleGenerator : IValueGenerator
    {
        public bool CanGenerate(Type Type)
        {
            return Type == typeof(double);
        }

        public object Generate(Type TypeToGenerate, GeneratorContext context)
        {
            return context.Random.NextDouble();
        }
    }
}

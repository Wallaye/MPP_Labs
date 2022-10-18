namespace Faker.Core.Generators
{
    internal class FloatGenerator : IValueGenerator
    {
        public bool CanGenerate(Type Type)
        {
            return Type == typeof(float);
        }

        public object Generate(Type TypeToGenerate, GeneratorContext context)
        {
            return context.Random.NextSingle();
        }
    }
}

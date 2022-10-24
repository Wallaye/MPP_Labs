using Faker.Core.Generators;

namespace Faker.Core.Tests.ConfigClasses
{
    internal class AgeGenerator : IValueGenerator
    {
        public bool CanGenerate(Type Type)
        {
            return typeof(int) == Type;
        }

        public object Generate(Type TypeToGenerate, GeneratorContext context)
        {
            return context.Random.Next(1, 100);
        }
    }
}

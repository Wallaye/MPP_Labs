using Faker.Core.Generators;

namespace Faker.Core.Tests.ConfigClasses
{
    internal class NameGenerator : IValueGenerator
    {
        public bool CanGenerate(Type Type)
        {
            return typeof(string) == Type;
        }

        public object Generate(Type TypeToGenerate, GeneratorContext context)
        {
            return "Name";
        }
    }
}

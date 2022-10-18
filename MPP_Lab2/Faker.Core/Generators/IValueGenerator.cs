namespace Faker.Core.Generators
{
    public interface IValueGenerator
    {
        object Generate(Type TypeToGenerate, GeneratorContext context);
        bool CanGenerate(Type Type);
    }
}
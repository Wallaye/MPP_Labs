namespace Faker.Core.Generators
{
    internal class BoolGenerator : IValueGenerator
    {
        public bool CanGenerate(Type Type)
        {
            return Type == typeof(bool);
        }

        public object Generate(Type TypeToGenerate, GeneratorContext context)
        {
            return Convert.ToBoolean(context.Random.Next(0, 2));
        }
    }
}

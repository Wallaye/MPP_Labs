namespace Faker.Core.Generators
{
    internal class CharGenerator : IValueGenerator
    {
        private static char[] _chars = 
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-=+~`'".ToCharArray();

        public bool CanGenerate(Type Type)
        {
            return Type == typeof(char);
        }

        public object Generate(Type TypeToGenerate, GeneratorContext context)
        {
            return _chars[context.Random.Next(_chars.Length)];
        }
    }
}

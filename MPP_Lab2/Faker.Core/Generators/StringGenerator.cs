using System.Text;

namespace Faker.Core.Generators
{
    internal class StringGenerator : IValueGenerator
    {
        private static char[] _chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-=+~`'".ToCharArray();
        private StringBuilder _sb = new();
        public bool CanGenerate(Type Type)
        {
            return Type == typeof(string);
        }

        public object Generate(Type TypeToGenerate, GeneratorContext context)
        {
            int length = context.Random.Next(3, 100);
            _sb.Clear();
            for (int i = 0; i < length; i++)
            {
                _sb.Append(_chars[context.Random.Next(0, _chars.Length)]);
            }
            return _sb.ToString();
        }
    }
}

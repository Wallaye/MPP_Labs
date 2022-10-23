using Faker.Core.Exceptions;
using System.Collections;

namespace Faker.Core.Generators
{
    public class ListGenerator : IValueGenerator
    {
        public object Generate(Type typeToGenerate, GeneratorContext context)
        {
            var listSize = context.Random.Next(1, 20);
            var list = (IList?)Activator.CreateInstance(typeToGenerate, listSize);
            if (list != null)
            {
                var argument = typeToGenerate.GetGenericArguments()[0];
                for (int i = 0; i < listSize; i++)
                {
                    list.Add(context.Faker.Create(argument));
                }
                return list;
            }
            throw new TypeException($"Can't generate instance of{typeToGenerate.Name}", typeToGenerate);
        }

        public bool CanGenerate(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}
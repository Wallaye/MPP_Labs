namespace Faker.Core.Exceptions
{
    public class TypeException : Exception
    {
        public Type Type;

        public TypeException(string message, Type type) : base(message)
        {
            Type = type;
        }
    }
}

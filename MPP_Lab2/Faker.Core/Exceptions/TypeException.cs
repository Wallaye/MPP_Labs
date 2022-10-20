namespace Faker.Core.Exceptions
{
    internal class TypeException : Exception
    {
        public Type Type;

        public TypeException(Type type, string message) : base(message)
        {
            Type = type;
        }
    }
}

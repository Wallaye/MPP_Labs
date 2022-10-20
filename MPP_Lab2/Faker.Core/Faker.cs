namespace Faker.Core
{
    public class Faker : IFaker
    {
        public T Create<T>()
        {
            throw new NotImplementedException();
        }

        public object Create(Type type)
        {
            throw new NotImplementedException();
        }

        public object CreateByName(Type type, string name)
        {
            throw new NotImplementedException();
        }
    }
}

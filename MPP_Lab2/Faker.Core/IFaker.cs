namespace Faker.Core
{
    public interface IFaker
    {
        public IFakerConfig? Config { get; }
        public T Create<T>();
        public object Create(Type type);
        public object CreateByName(Type type, string name);
    }
}

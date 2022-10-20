namespace Faker.Core
{
    public interface IFaker
    {
        //FakerConfig Config { get; }
        public T Create<T>();
        public object Create(Type type);
        public object CreateByName(Type type, string name);
    }
}

using Faker.Core.Generators;
using System.Reflection;
using Faker.Core.Exceptions;

namespace Faker.Core
{
    public class Faker : IFaker
    {
        private readonly GeneratorContext _generatorContext;
        private readonly List<IValueGenerator> _valueGenerators;
        public IFakerConfig? Config { get; }

        public Faker()
        {
            _generatorContext = new(
                new Random((int)DateTime.Now.Ticks),
                this
            );
            _valueGenerators = GetGenerators(Assembly.GetExecutingAssembly());
            AddGenerators("ListGenerator.dll");
            AddGenerators("DateTimeGenerator.dll");
        }

        public Faker(IFakerConfig config)
            : this()
        {
            Config = config;
        }

        public T Create<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        public object Create(Type type)
        {
            return CreateInstance(type);
        }

        private object CreateInstance(Type type)
        {
            foreach (var generator in _valueGenerators)
            {
                if (generator.CanGenerate(type))
                {
                    return generator.Generate(type, _generatorContext);
                }
            }
            throw new TypeException($"Can't create instance of {type.Name}", type);
        }

        public object CreateByName(Type type, string name)
        {
            if (Config != null)
            {
                var generator = Config.GetGenerator(name);
                if (generator != null)
                {
                    return generator.Generate(type, _generatorContext);
                }
            }
            return CreateInstance(type);
        }

        private List<IValueGenerator>? GetGeneratorsFromAssembly(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            if (assembly != null)
            {
                var generators = GetGenerators(assembly);
                return generators;
            }
            return null;
        }

        private List<IValueGenerator> GetGenerators(Assembly assembly)
        {
            var generatorsList = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IValueGenerator)) && t.IsClass)
                .Select(t => (IValueGenerator)Activator.CreateInstance(t)).ToList();
            return generatorsList;
        }

        public void AddGenerators(string assemblyPath)
        {
            var list = GetGeneratorsFromAssembly(assemblyPath);
            if (list != null)
            {
                list.AddRange(_valueGenerators);
                _valueGenerators.Clear();
                _valueGenerators.AddRange(list);
            }
        }
    }
}

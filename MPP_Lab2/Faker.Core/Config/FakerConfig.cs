using Faker.Core.Generators;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Faker.Core.Config
{
    public class FakerConfig : IFakerConfig
    {
        private Dictionary<string, IValueGenerator> _generators = new();

        public void Add<T, M, G>(Expression<Func<T, M>> expression) where G : IValueGenerator
        {
            try
            {
                MemberExpression member = (MemberExpression)expression.Body;

                G? generator = (G)Activator.CreateInstance(typeof(G));
                if (generator != null)
                {
                    _generators.Add(typeof(T).FullName + '.' + member.Member.Name.ToLower(), generator);
                }
            }
            catch
            {
                throw new Exception("Can't add generator to config");
            }
        }

        public IValueGenerator GetGenerator(string name)
        {
            if (_generators.TryGetValue(name, out IValueGenerator generator))
            {
                return generator;
            }
            return null;
        }
    }
}

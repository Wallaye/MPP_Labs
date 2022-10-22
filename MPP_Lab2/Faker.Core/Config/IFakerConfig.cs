using Faker.Core.Generators;
using System.Linq.Expressions;

namespace Faker.Core
{
    public interface IFakerConfig
    {
        void Add<T, M, G>(Expression<Func<T, M>> expression) where G : IValueGenerator;
        IValueGenerator GetGenerator(string memberName);
    }
}

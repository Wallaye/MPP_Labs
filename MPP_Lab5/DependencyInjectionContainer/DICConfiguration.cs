using System.Reflection.Metadata;

namespace DependencyInjectionContainer;

public class DICConfig
{
    public readonly List<DependencyItem> DefaultDependencies = new();
    public readonly Dictionary<object, List<DependencyItem>> Dependencies = new();

    public void Register<TDep, TImpl>(object? val = default, bool isSingleton = false)  
    {
        Register(typeof(TDep), typeof(TImpl), val, isSingleton);
    }
    
    public void Register(Type dep, Type impl, object? val = default, bool isSingleton = false)
    {
        List<DependencyItem> dependencyItems;
        if (val == null)
        {
            dependencyItems = DefaultDependencies;
        }
        else
        {
            if (!Dependencies.ContainsKey(val))
            {
                Dependencies.Add(val, new List<DependencyItem>());
            }
            dependencyItems = Dependencies[val];
        }
        
        if (dependencyItems.Any(d => d.DependencyType == dep && d.ImplementationType == impl))
        {
            dependencyItems.First().IsSingleton = isSingleton;
        }
        else
        {
            if (dep.IsGenericType)
            {
                dep = dep.GetGenericTypeDefinition();
            }

            var item = new DependencyItem(dep, impl, isSingleton);
            dependencyItems.Add(item);
        }
    }
}
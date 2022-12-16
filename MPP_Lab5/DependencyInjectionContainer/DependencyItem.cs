namespace DependencyInjectionContainer;

public class DependencyItem
{
    public object SingletonImplementation { get; set; }

    public Type DependencyType { get; }

    public Type ImplementationType { get; }

    public bool IsSingleton { get; set; }

    public DependencyItem(Type dependency, Type implementation, bool isSingleton)
    {
        DependencyType = dependency;
        ImplementationType = implementation;
        IsSingleton = isSingleton;
    }
}
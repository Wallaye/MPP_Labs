namespace DependencyInjectionContainer;

[AttributeUsage(AttributeTargets.Parameter)]
public class DependencyKeyAttribute : Attribute
{
    public object enumVal { get; }

    public DependencyKeyAttribute(object enumVal)
    {
        this.enumVal = enumVal;
    }
}
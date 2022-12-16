using System.Runtime.InteropServices;
using DependencyInjectionContainer;

namespace DICTests;

public class Tests
{
    private DICConfig config;
    private DiProvider provider;
    

    [SetUp]
    public void SetUp()
    {
        config = new DICConfig();
    }

    [Test]
    public void TestInterfaceClass()
    {
        config.Register<IInterface1, Class1>();
        provider = new DiProvider(config);
        var actual = provider.Resolve<IInterface1>();
        actual.Method1();
    }
    [Test]
    public void TestSingletonClass()
    {
        config.Register<IInterface1, Class1>(isSingleton: true);
        provider = new DiProvider(config);
        var actual = provider.Resolve<IInterface1>();
        var expected = provider.Resolve<IInterface1>();
        Assert.AreEqual(expected, actual);
    }
    [Test]
    public void TestIEnumerableClasses()
    {
        config.Register<IInterface1, Class1>();
        config.Register<IInterface1, Class2>();
        provider = new DiProvider(config);
        var actual = provider.Resolve<IEnumerable<IInterface1>>();
        Assert.AreEqual(2, actual.ToArray().Length);
    }
    [Test]
    public void TestAbstractClass()
    {
        config.Register<AbstractClass, Class3>();
        provider = new DiProvider(config);
        var actual = provider.Resolve<AbstractClass>();
        actual.Method2();
    }
    [Test]
    public void TestReflectedClass()
    {
        config.Register<Class3, Class4>();
        provider = new DiProvider(config);
        var actual = provider.Resolve<Class3>();
        actual.Method2();
    }
    [Test]
    public void TestGenericClass()
    {
        config.Register(typeof(IService<>), typeof(GenericClass<>));
        provider = new DiProvider(config);
        var obj = provider.Resolve<GenericClass<IRepository>>();
        var actual = obj.ServiceMethod();
        IRepository expected = default;
        Assert.AreEqual(expected, actual);
    }
    [Test]
    public void TestConstructorClass()
    {
        config.Register<IInterface2, Class5>();
        config.Register(typeof(IService<>), typeof(GenericClass<>));
        provider = new DiProvider(config);
        var obj = provider.Resolve<IInterface2>();
        var actual = obj.RetServ();
        IRepository expected = default;
        Assert.AreEqual(expected, actual);
    }
    [Test]
    public void TestIncorectDependency()
    {
        bool pass = false;
        config.Register<AbstractClass, AbstractClass2>();
        try
        {
            provider = new DiProvider(config);
        }
        catch(Exception e)
        {
            pass = true;
        }
        Assert.IsTrue(pass);
    }
    [Test]
    public void TestEnumsToClasses()
    {
        config.Register<IInterface1, Class1>(Interface1Implementations.First);
        config.Register<IInterface1, Class2>(Interface2Implementations.First);
        config.Register<IInterface1, Class2>(Interface1Implementations.Second);
        config.Register<IInterface2, Class5>(Interface2Implementations.First);
        config.Register(typeof(IService<>), typeof(GenericClass<>));

        provider = new DiProvider(config);
        Assert.AreEqual(1, provider.Resolve<IEnumerable<IInterface1>>(Interface1Implementations.First).Count());
        Assert.AreEqual(1, provider.Resolve<IEnumerable<IInterface1>>(Interface2Implementations.First).Count());
        Assert.AreEqual(1, provider.Resolve<IEnumerable<IInterface1>>(Interface1Implementations.Second).Count());
        Assert.AreEqual(1, provider.Resolve<IEnumerable<IInterface2>>(Interface2Implementations.First).Count());
    }
    [Test]
    public void TestAttributesInContructors()
    {
        config.Register<IInterface3, ClassInt3>(Interface3Implementations.First);
        config.Register<IInterface3, ClassInt4>(Interface3Implementations.Second);
        config.Register<IInterface3, ClassInt>();

        provider = new DiProvider(config);
        var obj = provider.Resolve<IInterface3>();
        Assert.AreEqual(4, obj.RetInt());
    }
}
using System.Reflection;
using AssemblyAnalyzer;
using NUnit.Framework.Constraints;

namespace AssemblyAnalyzerTests;

public class Tests
{
    private Analyzer _analyzer;
    [SetUp]
    public void Setup()
    {
        _analyzer = new Analyzer();
        _analyzer.SetAssembly(Assembly.LoadFrom(@"D:\Faker.Core.dll"));
    }

    [Test]
    public void TestCountingNamespaces()
    {
        var result = _analyzer.Analyze();
        Assert.That(result.Namespaces.Count, Is.EqualTo(4));
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void TestFindingClasses(int index)
    {
        var result = _analyzer.Analyze();
        Assert.That(result.Namespaces[index].Classes.Count > 0);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(7)]
    [TestCase(8)]
    [TestCase(9)]
    [TestCase(10)]
    public void TestClassInheritance(int index)
    {
        var result = _analyzer.Analyze();
        var type = result.Asm.GetType("Faker.Core.Generators.IValueGenerator");
        if (type == null) Assert.Fail();
        var namespaceData = result.Namespaces.Find(n => n.Name == "Faker.Core.Generators");
        if (namespaceData == null) Assert.Fail();
        Assert.IsInstanceOf(type!, Activator.CreateInstance(namespaceData!.Classes[index].ClassType.UnderlyingSystemType));
    }

    [Test]
    public void TestClassForMembers()
    {
        var result = _analyzer.Analyze();
        var namespaceData = result.Namespaces.Find(n => n.Name.Equals("Faker.Core"));
        var classData = namespaceData.Classes.Find(c => c.ClassType.Name == "Faker");
        Assert.Multiple(() =>
        {
            Assert.That(classData.Constructors.Count > 0);
            Assert.That(classData.Methods.Count > 0);
            Assert.That(classData.Fields.Count > 0);
            Assert.That(classData.Properties.Count > 0);
        });
    }
}
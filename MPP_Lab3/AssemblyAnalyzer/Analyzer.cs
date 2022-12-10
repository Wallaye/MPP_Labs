using System.Reflection;
using System.Runtime.CompilerServices;
using AssemblyAnalyzer.Data;

namespace AssemblyAnalyzer;

public class Analyzer
{
    private AssemblyData asm;
    public void SetAssembly(Assembly asm)
    {
        this.asm = new AssemblyData(asm);
    }
    public AssemblyData Analyze()
    {
        GetNamespaces();
        GetClasses();
        return asm;
    }

    private void AddNamespace(Type type)
    {
        var list = asm.Namespaces;
        string? name = type.Namespace;
        if (asm.Namespaces.Any(n => n.Name == name || name == null)) return;
        list.Add(new(name));
    }

    private void GetNamespaces()
    {
        foreach (var type in asm.Asm.GetTypes())
        {
            if (Attribute.GetCustomAttribute(type, typeof(CompilerGeneratedAttribute)) == null)
                AddNamespace(type);
        }
    }

    private void GetClasses()
    {
        for (int i = 0; i < asm.Namespaces.Count; i++)
        {
            var @namespace = asm.Namespaces[i];
            var types = asm.Asm.GetTypes().Select(t => t)
                .Where(t => t.Namespace == @namespace.Name && Attribute.GetCustomAttribute(t, typeof(CompilerGeneratedAttribute)) == null)
                .ToList();
            foreach (var type in types)
            {
                @namespace.Classes.Add(new ClassData(type));
            }

            foreach (var classData in @namespace.Classes)
            {
                GetClassMembers(classData);
            }
        }
    }

    private void GetClassMembers(ClassData classData)
    {
        if (!classData.IsExtension)
        {
            classData.Constructors.AddRange(classData.ClassType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(c => Attribute.GetCustomAttribute(c, typeof(CompilerGeneratedAttribute)) == null));
                    
            classData.Properties.AddRange(classData.ClassType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(p => Attribute.GetCustomAttribute(p, typeof(CompilerGeneratedAttribute)) == null));

            classData.Fields.AddRange(classData.ClassType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(f => Attribute.GetCustomAttribute(f, typeof(CompilerGeneratedAttribute)) == null));
        }
        
        foreach (var methodInfo in classData.ClassType.
                     GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
        {
            if (Attribute.GetCustomAttribute(methodInfo, typeof(CompilerGeneratedAttribute)) != null) continue;
            if (IsExtensionMethod(methodInfo))
            {
                Type extensionType = methodInfo.GetParameters()[0].ParameterType;
                AddNamespace(extensionType);
                NamespaceData namespaceData = asm.Namespaces.First(n => n.Name == extensionType.Namespace);
                ClassData extensionClassData = new(extensionType, true);
                if (!namespaceData.Classes.Any(c => c.ClassType.Name == extensionClassData.ClassType.Name))
                {
                    namespaceData.Classes.Add(extensionClassData);
                }
                else
                {
                    extensionClassData =
                        namespaceData.Classes.First(c => c.ClassType.Name == extensionClassData.ClassType.Name);
                }
                
                extensionClassData.Methods.Add(new MethodData(methodInfo, true));
            }
            else
            {
                if (!classData.IsExtension)
                {
                    classData.Methods.Add(new MethodData(methodInfo, false));
                }
            }
        }
    }

    private static bool IsExtensionMethod(MethodInfo methodInfo)
    {
        return methodInfo.IsDefined(typeof(ExtensionAttribute), false);
    }
}
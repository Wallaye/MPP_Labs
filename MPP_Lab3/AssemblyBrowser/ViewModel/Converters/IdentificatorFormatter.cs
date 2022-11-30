using AssemblyAnalyzer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowser.ViewModel.Converters
{
    public static class IdentificatorFormatter
    {
        public static string GetClassIndentificator(ClassData classData)
        {
            string result = "";
            if (classData.ClassType.IsPublic) result += "public";
            else if (classData.ClassType.IsNestedPrivate) result += "private";
            else if (classData.ClassType.IsNestedFamily) result += "protected";
            else if (classData.ClassType.IsNestedAssembly) result += "internal";
            else if (classData.ClassType.IsNestedFamANDAssem) result += "protected internal";
            else if (classData.ClassType.IsNotPublic) result += "private";
            if (classData.ClassType.IsAbstract && classData.ClassType.IsSealed)
                result += " static";
            else if (classData.ClassType.IsAbstract && !classData.ClassType.IsInterface)
                result += " abstract";
            if (classData.ClassType.IsClass)
                result += " class";
            if (classData.ClassType.IsEnum)
                result += " enum";
            if (classData.ClassType.IsInterface)
                result += " interface";
            if (classData.ClassType.IsValueType && classData.ClassType.IsPrimitive)
                result += " struct";
            return result;
        }

        public static string GetMethodIdentificator(MethodInfo m)
        {
            string result = "";
            if (m.IsPublic) result += "public";
            else if (m.IsPrivate) result += "private";
            else if (m.IsFamily) result += "protected";
            else if (m.IsAssembly) result += "internal";
            else if (m.IsFamilyOrAssembly) result += "protected internal";
            if (m.IsStatic) result += " static";
            else if (m.IsAbstract) result += " abstract";
            else if (m.IsVirtual) result += " virtual";
            else if (m.GetBaseDefinition() != m) result += " override";
            return result;
        }

        public static string GetFieldIdentificator(FieldInfo f)
        {
            if (f == null) return "";
            string result = "";
            if (f.IsPublic) result += "public";
            else if (f.IsPrivate) result += "private";
            else if (f.IsFamily) result += "protected";
            else if (f.IsAssembly) result += "internal";
            else if (f.IsFamilyOrAssembly) result += "protected internal";
            if (f.IsStatic) result += " static";
            return result;
        }

        public static string GetCtorIdentificator(ConstructorInfo c)
        {
            string result = "";
            if (c.IsPublic) result += "public";
            else if (c.IsPrivate) result += "private";
            else if (c.IsFamily) result += "protected";
            else if (c.IsAssembly) result += "internal";
            else if (c.IsFamilyOrAssembly) result += "protected internal";
            return result;
        }
    }
}

using Faker.Core.Exceptions;

namespace Faker.Core.Generators
{
    internal class UserTypeGenerator : IValueGenerator
    {
        private readonly int _recursiveLimit = 1;
        private List<Type> _types = new List<Type>();

        public bool CanGenerate(Type Type)
        {
            return Type.IsClass || (Type.IsValueType && !Type.IsEnum);
        }

        public object Generate(Type TypeToGenerate, GeneratorContext context)
        {
            object obj = CreateObject(TypeToGenerate, context);
            _types.Add(TypeToGenerate);
            InitFields(obj, TypeToGenerate, context);
            InitProperties(obj, TypeToGenerate, context);
            _types.Remove(TypeToGenerate);
            return obj;
        }

        public object CreateObject(Type type, GeneratorContext context)
        {
            var constructors = type.GetConstructors()
                                   .OrderByDescending(c => c.GetParameters().Length)
                                   .ToArray();
            try
            {
                foreach (var constructor in constructors)
                {
                    var parameters = constructor.GetParameters()
                                                .Select(p => context.Faker.CreateByName(p.ParameterType,
                                                    type.FullName + "." + p.Name.ToLower()))
                                                .ToArray();
                    return constructor.Invoke(parameters);
                }
            }
            catch { }
            try
            {
                var obj = Activator.CreateInstance(type);
                if (obj != null) 
                {
                    return obj;
                }
            }
            catch { }
            throw new TypeException($"Cannot create instance of {type.Name}", type);
        }

        private void InitFields(object obj, Type type, GeneratorContext context)
        {
            var fields = type.GetFields().Where(p => !p.IsInitOnly);
            foreach (var field in fields)
            {
                try
                {
                    if (Equals(field.GetValue(obj), GetDefaultValue(field.FieldType)))
                    {
                        if (!CanInit(field.FieldType))
                            continue;
                        field.SetValue(obj, context.Faker
                            .CreateByName(field.FieldType, type.Name + "." + field.Name.ToLower()));
                    };
                }
                catch { }
            }
        }

        private void InitProperties(object obj, Type type, GeneratorContext context)
        {
            var properties = type.GetProperties().Where(p => p.CanWrite);
            foreach(var property in properties)
            {
                try
                {
                    if (Equals(property.GetValue(obj), GetDefaultValue(property.PropertyType)))
                    {
                        if (!CanInit(property.PropertyType))
                            continue;
                        property.SetValue(obj, context.Faker
                            .CreateByName(property.PropertyType, type.Name + "." + property.Name.ToLower()));
                    };
                }
                catch { }
            }
        }

        private bool CanInit(Type type) =>
            _types.Where(p => p == type).Count() <= _recursiveLimit;

        private static object? GetDefaultValue(Type type) => 
            type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}

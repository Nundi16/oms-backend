using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace OMS.Presentation.Extensions
{
    public static class JsonSerializerOptionsExtensions
    {
        public static JsonSerializerOptions WithPolymorhicModifiersOf<TAncestor>(this JsonSerializerOptions options)
        {
            options.TypeInfoResolver ??= new DefaultJsonTypeInfoResolver();
            options.TypeInfoResolver = options.TypeInfoResolver.WithAddedModifier(GetPolymorhicModifier<TAncestor>);

            return options;
        }

        private static void GetPolymorhicModifier<TAncestor>(JsonTypeInfo typeInfo)
        {
            var ancestor = typeof(TAncestor);

            var derivedTypes = GetDescendantsFromAssebmly(ancestor);

            if (typeInfo.Type == ancestor)
            {
                typeInfo.PolymorphismOptions = new()
                {
                    TypeDiscriminatorPropertyName = "TypeDescriptor"
                };

                foreach (var type in derivedTypes)
                {
                    typeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(type, type.Name));
                }
            }
        }

        private static TypeInfo[] GetDescendantsFromAssebmly(Type type)
        {
            return [..
                type.Assembly.DefinedTypes.Where(descendantType =>
                    descendantType != type &&
                    !descendantType.IsAbstract &&
                    !descendantType.IsInterface &&
                    type.IsAssignableFrom(descendantType.AsType()))
            ];
        }
    }
}

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace OMS.Presentation.Serialization
{
    public static class PolymorphicJsonOption
    {
        /// <summary>
        /// Applies polymorphic configuration for the given base type onto an existing
        /// JsonSerializerOptions instance (used from AddJsonOptions).
        /// </summary>
        public static void Configure(JsonSerializerOptions target, Type polimorphicBaseType)
        {
            var derivedTypes = polimorphicBaseType.Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && polimorphicBaseType.IsAssignableFrom(t) && t != polimorphicBaseType)
                .ToList();

            target.PropertyNameCaseInsensitive = false;

            var resolver = target.TypeInfoResolver as DefaultJsonTypeInfoResolver
                ?? new DefaultJsonTypeInfoResolver();

            resolver.Modifiers.Add(ti =>
            {
                if (ti.Type == polimorphicBaseType)
                {
                    ti.PolymorphismOptions = new JsonPolymorphismOptions
                    {
                        TypeDiscriminatorPropertyName = "Descriptor",
                        IgnoreUnrecognizedTypeDiscriminators = false
                    };

                    foreach (var type in derivedTypes)
                    {
                        ti.PolymorphismOptions.DerivedTypes.Add(
                            new JsonDerivedType(type, type.Name));
                    }
                }
            });

            target.TypeInfoResolver = resolver;
        }
    }
}

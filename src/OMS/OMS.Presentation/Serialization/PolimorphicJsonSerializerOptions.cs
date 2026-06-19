using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace OMS.Presentation.Serialization
{
    public static class PolymorphicJsonOption
    {
        /// <summary>
        /// Builds JsonSerializerOptions that enable polymorphic (de)serialization for
        /// the given base type and all of its concrete subtypes found in the same
        /// assembly as the base. Uses "Type" as the discriminator property and the
        /// short type name as the discriminator value, so the frontend can send
        /// e.g. { "Type": "OrderClinicDto", ... } inside a Connectors array.
        /// </summary>
        public static JsonSerializerOptions GetOptions(Type polimorphicBaseType)
        {
            // Discover derived types from the base type's own assembly (Application),
            // not from the Presentation assembly.
            var derivedTypes = polimorphicBaseType.Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && polimorphicBaseType.IsAssignableFrom(t) && t != polimorphicBaseType)
                .ToList();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                {
                    Modifiers =
                    {
                        ti =>
                        {
                            if (ti.Type == polimorphicBaseType)
                            {
                                ti.PolymorphismOptions = new JsonPolymorphismOptions
                                {
                                    TypeDiscriminatorPropertyName = "Type",
                                    IgnoreUnrecognizedTypeDiscriminators = false
                                };

                                foreach (var type in derivedTypes)
                                {
                                    ti.PolymorphismOptions.DerivedTypes.Add(
                                        new JsonDerivedType(type, type.Name));
                                }
                            }
                        }
                    }
                }
            };

            return options;
        }

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

            target.PropertyNameCaseInsensitive = true;

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

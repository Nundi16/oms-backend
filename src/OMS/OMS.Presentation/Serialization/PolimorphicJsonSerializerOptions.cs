using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace OMS.Presentation.Serialization
{
    public static class PolymorphicJsonOption
    {
        public static JsonSerializerOptions GetOptions(Type polimorphicBaseType)
        {
            var derivedTypes = Assembly.GetExecutingAssembly()
               .GetTypes().Where(t => t.IsSubclassOf(polimorphicBaseType));

            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                {
                    Modifiers = 
                    {
                        ti => {
                            if (ti.Type == polimorphicBaseType)
                            {
                                ti.PolymorphismOptions = new()
                                {
                                    TypeDiscriminatorPropertyName = "descriptor"
                                };

                                foreach (var type in derivedTypes)
                                {
                                    ti.PolymorphismOptions.DerivedTypes.Add(
                                        new JsonDerivedType(type, type.FullName));
                                }
                            }
                        }
                    }
                }
            };

            return options;
        }
    }
}

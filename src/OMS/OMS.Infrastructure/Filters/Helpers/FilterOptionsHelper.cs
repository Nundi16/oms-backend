using System.Reflection;
using OMS.Common.Abstractions.Entity;
using OMS.Domain.Attributes;
using OMS.Infrastructure.Options;

namespace OMS.Infrastructure.Filters.Helpers
{
    internal static class FilterOptionsHelper
    {
        internal static FilterOption[] GetFilterOptionsFromCurrentAssembly()
        {
            var assembly = Assembly.GetCallingAssembly();

            var entityTypes = assembly.DefinedTypes
                .Where(typeInfo => typeInfo is { IsAbstract: false } && typeof(Entity).IsAssignableFrom(typeInfo));

            var typeInfos = entityTypes.Select(typeInfo => new 
            { 
                TypeName = typeInfo.Name, 
                Properties = GetPropertiesOfType(typeInfo).ExtractPropertyInfo() 
            });

            return [.. typeInfos.SelectMany(typeInfo => CreateFilterOptions(typeInfo.TypeName, typeInfo.Properties))];
        }

        private static IEnumerable<FilterOption> CreateFilterOptions(string typeName, IEnumerable<(string Name, string Type, FilterableAttribute FilterAttribute)> properties) =>
            properties.Select(property => new FilterOption
            {
                EntityName = typeName, 
                FieldName = property.Name, 
                FieldType = property.Type, 
                RequiresAuthorization = property.FilterAttribute.RequiresAuthorization,
                RequiresRole = property.FilterAttribute.RoleName
            });

        private static IEnumerable<MemberInfo> GetPropertiesOfType(TypeInfo type) =>
            [ .. type.DeclaredProperties, .. type.DeclaredFields ];

        private static IEnumerable<(string Name, string Type, FilterableAttribute FilterAttribute)> ExtractPropertyInfo(this IEnumerable<MemberInfo> properties) =>
            properties.Select(property => new
            {
                property.Name,
                Type = property.GetType().Name,
                Attribute = property.GetCustomAttribute<FilterableAttribute>()
            })
            .Where(propertyInfo => propertyInfo.Attribute is not null)
            .Select(propertyInfo =>
            (
                propertyInfo.Name,
                propertyInfo.Type,
                propertyInfo.Attribute!
            ));
    }
}

using System.Diagnostics.CodeAnalysis;

namespace OMS.Infrastructure.Options
{
    public sealed class FilterOptions
    {
        public required FilterOption[] Filters { get; init; }
    }

    public sealed record FilterOption
    {
        public required string EntityName { get; init; }

        public required string FieldName { get; init; }

        public required string FieldType { get; init; }

        [MemberNotNullWhen(true, nameof(RequiresRole))]
        public bool RequiresAuthorization { get; init; }

        public string? RequiresRole { get; init; }
    }
}

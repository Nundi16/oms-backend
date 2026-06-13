using System.Diagnostics.CodeAnalysis;

namespace OMS.Common.Interfaces
{
    public interface IResult
    {
        [MemberNotNullWhen(false, nameof(ErrorMessage))]
        bool Succeeded { get; }
        string? ErrorMessage { get; }
    }

    public interface IResult<TValue>
    {
        [MemberNotNullWhen(true, nameof(Value))]
        [MemberNotNullWhen(false, nameof(ErrorMessage))]
        bool Succeeded { get; }
        string? ErrorMessage { get; }
        TValue? Value { get; }
    }
}

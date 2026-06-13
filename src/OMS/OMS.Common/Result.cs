using OMS.Common.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace OMS.Common
{
    public class Result : IResult
    {
        [MemberNotNullWhen(false, nameof(ErrorMessage))]
        public bool Succeeded { get; private init; }
        public string? ErrorMessage { get; private init; }

        private Result(bool succeeded, string? errorMessage = null)
        {
            Succeeded = succeeded;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new(true);
        public static Result<TValue> Success<TValue>(TValue value) where TValue : notnull => Result<TValue>.Success(value);
        public static Result Failure(string message) => new(false, message);
        public static Result<TValue> Failure<TValue>(string message) where TValue : notnull => Result<TValue>.Failure(message);
    }

    public class Result<TValue> : IResult<TValue>
    {
        [MemberNotNullWhen(true, nameof(Value))]
        [MemberNotNullWhen(false, nameof(ErrorMessage))]
        public bool Succeeded { get; private init; }
        public TValue? Value { get; private init; }
        public string? ErrorMessage { get; private init; }

        private Result(bool succeeded, TValue? value, string? errorMessage = null)
        {
            Succeeded = succeeded;
            Value = value;
            ErrorMessage = errorMessage;
        }

        public static Result<TValue> Success(TValue value) => new(true, value);
        public static Result<TValue> Failure(string errorMessage) => new(false, default, errorMessage);
    }
}
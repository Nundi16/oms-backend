using OMS.Common.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace OMS.Common
{
    public class Result : IResult
    {
        public bool Succeeded { get; private set; }
        public string? ErrorMessage { get; private set; }
        private Result(bool isSuccess, string? errorMessage)
        {
            Succeeded = isSuccess;
            ErrorMessage = errorMessage;
        }
        public static Result Success() => new(true, null);
        public static Result Failure(string errorMessage) => new(false, errorMessage);
    }

    public class Result<T> : IResult<T>
    {
        [MemberNotNullWhen(true, nameof(Value))]
        public bool Succeeded { get; private set; }
        public T? Value { get; private set; }
        public string? ErrorMessage { get; private set; }

        private Result(bool isSuccess, T? value, string? errorMessage)
        {
            Succeeded = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
    }
}
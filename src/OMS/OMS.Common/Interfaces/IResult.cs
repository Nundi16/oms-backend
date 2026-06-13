namespace OMS.Common.Interfaces
{
    public interface IResult
    {
        bool Succeeded { get; }
        string? ErrorMessage { get; }
    }

    public interface IResult<T> : IResult
    {
        T? Value { get; }
    }
}

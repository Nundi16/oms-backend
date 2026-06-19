namespace OMS.Common.Interfaces.Communication.Handlers.Request
{
    public interface IRequestHandler<TRequest, TResponse> : IHandler where TRequest : class
    {
        Task<IResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }

    public interface IRequestHandler<TResponse> : IHandler
    {
        Task<IResult<TResponse>> HandleAsync(CancellationToken cancellationToken = default);
    }
}

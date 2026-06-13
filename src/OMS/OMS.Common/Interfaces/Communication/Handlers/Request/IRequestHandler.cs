namespace OMS.Common.Interfaces.Communication.Handlers.Request
{
    public interface IRequestHandler<TEvent, TResponse> : IHandler where TEvent : class
    {
        Task<IResult<TResponse>> HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}

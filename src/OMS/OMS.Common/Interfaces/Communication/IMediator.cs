namespace OMS.Common.Interfaces.Communication
{
    public interface IMediator
    {
        Task<IResult> EmitAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class;
        Task FanOutAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class;
        Task<IResult<TResponse>> RequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : class
            where TResponse : class;
    }
}

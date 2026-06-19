namespace OMS.Common.Interfaces.Communication
{
    public interface IMediator
    {
        Task EmitAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class;
        Task FanOutAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class;
        Task FanOutSequentialAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class;
        Task<IResult<TResponse>> RequestAsync<TEvent, TResponse>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class
            where TResponse : class;
    }
}

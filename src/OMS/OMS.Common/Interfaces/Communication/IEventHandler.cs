namespace OMS.Common.Interfaces.Communication
{
    public interface IHandler { }
    public interface IAuthorizedHandler
    {
        IResult Authorize();
    }

    public interface IEventHandler<in TEvent> : IHandler where TEvent : class
    {
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }

    public interface IAuthorizedEventHandler<in TEvent> : IAuthorizedHandler, IHandler where TEvent : class;
    public interface IRequestHandler<in TEvent, TResponse> : IHandler where TEvent : class
    {
        Task<IResult<TResponse?>> HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }

    public interface IAuthorizedRequestHandler<in TEvent, TResponse> : IRequestHandler<TEvent, TResponse>, IAuthorizedEventHandler<TEvent> where TEvent : class;
}

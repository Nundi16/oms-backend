namespace OMS.Common.Interfaces.Communication.Handlers.Event
{
    public interface IEventHandler<TEvent> : IHandler where TEvent : class
    {
        Task<IResult> HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}

namespace OMS.Common.Interfaces.Communication
{
    public interface IMediatorAuthorizationGuard
    {
        IResult Authorize<TEventHandler, TEvent>(TEventHandler handler)
            where TEventHandler : IHandler
            where TEvent : class;
    }
}

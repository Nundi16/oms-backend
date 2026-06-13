namespace OMS.Common.Interfaces.Communication.Handlers
{
    public interface IAuthorizedHandler : IHandler
    {
        IResult Authorize();
    }
}

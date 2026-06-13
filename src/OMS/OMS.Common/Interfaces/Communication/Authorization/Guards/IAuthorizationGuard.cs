namespace OMS.Common.Interfaces.Communication.Authorization.Guards
{
    public interface IAuthorizationGuard
    {
        IResult Authorize();
    }
}

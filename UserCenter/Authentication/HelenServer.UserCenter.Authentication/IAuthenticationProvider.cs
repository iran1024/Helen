using HelenServer.Core;
using HelenServer.UserCenter.Shared;

namespace HelenServer.Authentication.OAuth2
{
    public interface IAuthenticationProvider
    {
        Task<OperationResult<LoginResult>> ValidAsync(ValidModel validModel, CancellationToken cancellationToken);
    }
}
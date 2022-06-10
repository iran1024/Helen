using HelenServer.Core;
using HelenServer.UserCenter.Shared;

namespace HelenServer.Authentication.OAuth2
{
    public interface IAuthenticationService
    {
        Task<OperationResult<LoginResult>> LoginAsync(LoginModel loginModel, CancellationToken cancellationToken);

        Task<OperationResult<LogoutModel>> LogoutAsync(LogoutModel loggedOutModel, CancellationToken cancellationToken);
    }
}

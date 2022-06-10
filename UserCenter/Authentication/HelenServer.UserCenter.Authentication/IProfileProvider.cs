using IdentityServer4.Models;

namespace HelenServer.Authentication.OAuth2
{
    public interface IProfileProvider
    {
        Task SetUserProfileAsync(ProfileDataRequestContext context, CancellationToken cancellationToken);
    }
}
using HelenServer.Core;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Security.Claims;

namespace HelenServer.Authentication.OAuth2
{
    internal class UserProfileService : IProfileService
    {
        private readonly IServiceProvider _provider;

        public UserProfileService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string? source = context.Subject.FindFirstValue("source");

            var profileProvider = _provider.GetRequiredService<IProfileProvider>(source);

            await profileProvider.SetUserProfileAsync(context, CancellationToken.None);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}

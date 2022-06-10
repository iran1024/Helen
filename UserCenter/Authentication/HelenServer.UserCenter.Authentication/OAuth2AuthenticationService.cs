using HelenServer.Core;
using HelenServer.UserCenter.Shared;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace HelenServer.Authentication.OAuth2
{
    [ServiceProvider(typeof(IAuthenticationService), "OAuth2")]
    internal class OAuth2AuthenticationService : IAuthenticationService
    {
        internal const string SOURCE_PREFIX = "source:";

        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHttpContextAccessor _accessor;
        private readonly IEventService _events;

        public OAuth2AuthenticationService(IIdentityServerInteractionService interaction, IHttpContextAccessor accessor, IEventService events)
        {
            _interaction = interaction;
            _accessor = accessor;
            _events = events;
        }

        public async Task<OperationResult<LoginResult>> LoginAsync(LoginModel loginModel, CancellationToken cancellationToken)
        {
            var context = await GetSpaContextAsync(loginModel.ReturnUrl);

            string? source = context.AcrValues.FirstOrDefault(x => x.StartsWith(SOURCE_PREFIX));

            if (string.IsNullOrWhiteSpace(source))
            {
                source = $"{SOURCE_PREFIX}platform";
            }

            source = source[SOURCE_PREFIX.Length..];

            var authentication = _accessor.HttpContext!.RequestServices
                .GetRequiredService<IAuthenticationProvider>(source);

            var validModel = new ValidModel
            {
                LoginModel = loginModel,
                Source = source
            };

            return await authentication.ValidAsync(validModel, cancellationToken);
        }

        public async Task<OperationResult<LogoutModel>> LogoutAsync(LogoutModel model, CancellationToken cancellationToken)
        {
            if (model.ShowLogoutPrompt)
            {
                if (_accessor.HttpContext!.User?.Identity?.IsAuthenticated != true)
                {
                    model.ShowLogoutPrompt = false;
                }

                var context = await _interaction.GetLogoutContextAsync(model.LogoutId);

                if (context?.ShowSignoutPrompt == false)
                {
                    model.ShowLogoutPrompt = false;
                }

                if (model.ShowLogoutPrompt == false)
                {
                    return await LogoutAsync(model.LogoutId, CancellationToken.None);
                }

                return OperationResult<LogoutModel>.OK(model);
            }
            else
            {
                return await LogoutAsync(model.LogoutId, CancellationToken.None);
            }
        }

        private async Task<OperationResult<LogoutModel>> LogoutAsync(string logoutId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            if (logout == null)
            {
                return OperationResult<LogoutModel>.Failed("无法识别的ID");
            }

            var model = new LogoutModel
            {
                LogoutId = logoutId,
                LogoutIframeUrl = logout.SignOutIFrameUrl,
                PostLogoutRedirectUri = logout.PostLogoutRedirectUri,
            };

            if (_accessor.HttpContext!.User.Identity?.IsAuthenticated == true)
            {
                string? idp = _accessor.HttpContext.User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

                if (idp is not null and not IdentityServerConstants.LocalIdentityProvider)
                {
                    bool providerSupportsSignout =
                        await _accessor.HttpContext.GetSchemeSupportsSignOutAsync(idp);

                    if (providerSupportsSignout)
                    {
                        if (model.LogoutId == null)
                        {
                            model.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        model.ExternalAuthenticationScheme = idp;
                    }
                }

                await _accessor.HttpContext.SignOutAsync();

                string? subjectId = _accessor.HttpContext.User.GetSubjectId();
                string? displayName = _accessor.HttpContext.User.GetDisplayName();

                await _events.RaiseAsync(new UserLogoutSuccessEvent(subjectId, displayName));
            }

            return OperationResult<LogoutModel>.OK(model);
        }

        private async Task<AuthorizationRequest> GetSpaContextAsync(string returnUrl)
        {
            string? url = returnUrl;

            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith("http"))
            {
                int urlIndex = returnUrl.IndexOf("/connect/authorize/callback?");

                if (urlIndex > 0)
                {
                    url = url[urlIndex..];
                }
            }

            var context = await _interaction.GetAuthorizationContextAsync(url);

            return context;
        }
    }
}

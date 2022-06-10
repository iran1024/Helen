using HelenServer.Authentication.OAuth2;
using HelenServer.UserCenter.Authentication;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceConllectionExtensions
    {
        public static IIdentityServerBuilder AddCloudIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalApiAuthentication(principal =>
            {
                var identity = principal.Identities.First();

                identity.AddClaim(new Claim(ClaimTypes.Name, principal.FindFirstValue(JwtClaimTypes.Name) ?? string.Empty));

                identity.AddClaim(new Claim(ClaimTypes.Role, principal.FindFirstValue(JwtClaimTypes.Role) ?? string.Empty));

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.FindFirstValue(JwtClaimTypes.Subject) ?? string.Empty));

                return Task.FromResult(principal);
            });

            var builder = services
                .AddIdentityServer(options =>
                {
                    configuration.Bind("IdentityServer", options);
                })
                .AddClientStore<ClientStore>()
                .AddResourceStore<ResourceStore>()
                .AddPersistedGrantStore<PersistedGrantStore>()
                .AddProfileService<UserProfileService>();

            var certFile = configuration.GetSection("IdentityServer:CertFile");

            if (certFile.Exists())
            {
                var credential = X509Certificate2.CreateFromPemFile(
                    certFile.GetValue<string>("Path"),
                    certFile.GetValue<string>("KeyPath"));

                builder.AddSigningCredential(credential);
            }
            else
            {
                builder.AddDeveloperSigningCredential();
            }

            services.AddAuthentication()
                .AddCookie("cloud_cookie");

            return builder;
        }

        public static IApplicationBuilder UseCloudIdentityServer(this IApplicationBuilder app)
        {
            app.Use((context, next) =>
            {
                var configuration = context.RequestServices.GetRequiredService<IConfiguration>();

                var forwardOptions = context.RequestServices.GetRequiredService<IOptions<ForwardedHeadersOptions>>();

                if (context.Request.Headers.ContainsKey(forwardOptions.Value.OriginalHostHeaderName))
                {
                    string? basePath = $"/{configuration["Swagger:SwaggerUrlPrefix"]}";

                    context.Request.PathBase = new PathString(basePath);
                }

                return next();
            });

            app.UseIdentityServer();

            return app;
        }

        internal const SameSiteMode Unspecified = (SameSiteMode)(-1);

        public static IServiceCollection AddNonBreakingSameSiteCookies(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(opts =>
            {
                opts.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                opts.Secure = CookieSecurePolicy.Always;
                opts.OnAppendCookie = ctx => CheckSameSite(ctx.Context, ctx.CookieOptions);
                opts.OnDeleteCookie = ctx => CheckSameSite(ctx.Context, ctx.CookieOptions);
            });

            return services;
        }

        private static void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                string? userAgent = httpContext.Request.Headers["User-Agent"].ToString();

                if (DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = Unspecified;
                }
            }
        }

        private static bool DisallowsSameSiteNone(string userAgent)
        {
            if (userAgent.Contains("CPU iPhone OS 12") ||
                userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            if (userAgent.Contains("Safari") &&
                userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
                userAgent.Contains("Version/"))
            {
                return true;
            }

            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }
    }
}
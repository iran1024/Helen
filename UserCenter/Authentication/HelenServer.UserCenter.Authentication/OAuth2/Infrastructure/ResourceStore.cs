using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;

namespace HelenServer.Authentication.OAuth2
{
    public class ResourceStore : IResourceStore
    {
        private readonly ILogger<ResourceStore> _logger;
        private readonly IEnumerable<ApiScope> _apiScopes = new ApiScope[]
        {
            new ApiScope("helen_api", "Helen Api Scope", new string[] { JwtClaimTypes.Name, JwtClaimTypes.GivenName }),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

        public ResourceStore(ILogger<ResourceStore> logger)
        {
            _logger = logger;
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var results = _apiScopes.Where(s => scopeNames.Contains(s.Name));

            _logger.LogDebug("找到API Scopes：{scopes}", results.Select(x => x.Name));

            return Task.FromResult(results);
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var scopes = new List<string>();

            var identityResources = new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email()
                };

            var resources = identityResources.Where(x => scopeNames.Contains(x.Name));

            _logger.LogDebug("找到API Scopes：{scopes}", resources.Select(x => x.Name));

            return Task.FromResult(resources);
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var identityResources = new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email()
                };

            var result = new Resources(
                identityResources,
                new List<ApiResource>(),
                _apiScopes);

            _logger.LogDebug("找到所有的Scopes {scopes}",
                result.IdentityResources.Select(x => x.Name).Union(result.ApiResources.SelectMany(x => x.Scopes)));

            return Task.FromResult(result);
        }
    }
}

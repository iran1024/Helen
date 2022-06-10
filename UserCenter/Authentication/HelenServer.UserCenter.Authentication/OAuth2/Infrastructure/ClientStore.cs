using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace HelenServer.UserCenter.Authentication
{
    public class ClientStore : IClientStore
    {
        private readonly ICollection<Client> _clients = new List<Client>();

        public ClientStore()
        {
            _clients.Add(new Client()
            {
                ClientId = "helen",
                ClientName = "Helen System",
                ClientSecrets = { new Secret("F807D124-146B-46B4-AADF-41E01F292597".Sha256()) },
                ProtocolType = IdentityServerConstants.ProtocolTypes.OpenIdConnect,
                AccessTokenType = AccessTokenType.Jwt,
                AccessTokenLifetime = 30,
                AllowedGrantTypes = {
                    IdentityModel.OidcConstants.GrantTypes.AuthorizationCode,
                    IdentityModel.OidcConstants.GrantTypes.RefreshToken
                },
                AllowedScopes = new string[] {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.LocalApi.ScopeName,
                    "helen_api"
                },
                RequireClientSecret = true,
                UpdateAccessTokenClaimsOnRefresh = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowAccessTokensViaBrowser = true,
                AllowOfflineAccess = true,
            });
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _clients.FirstOrDefault(x => x.ClientId == clientId) ?? throw new Exception("客户端ID不存在");

            return Task.FromResult(client);
        }
    }
}
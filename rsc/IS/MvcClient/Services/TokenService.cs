using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace MvcClient.Services
{
    //to use this, we need to inject it in the Controller. Do not forget register this service in DI Container.
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private IOptions<IdentityServerOptions> _identityServerSettings;
        private DiscoveryDocumentResponse _discoveryDocument;

        public TokenService(ILogger<TokenService> logger, IOptions<IdentityServerOptions> identityServerSettings)
        {
            _logger = logger;
            _identityServerSettings = identityServerSettings;

            using var httpClient = new HttpClient();
            _discoveryDocument = httpClient.GetDiscoveryDocumentAsync(identityServerSettings.Value.DiscoveryUrl).Result;
            if (_discoveryDocument.IsError)
            {
                logger.LogError($"Unable to get discovery document. Error is: {_discoveryDocument.Error}");
                throw new Exception("Unable to get discovery document", _discoveryDocument.Exception);
            }
        }

        public async Task<TokenResponse> GetTokenAsync(string scope)
        {
            using var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
            Address = _discoveryDocument.TokenEndpoint,
            ClientId = _identityServerSettings.Value.ClientName,
            ClientSecret = _identityServerSettings.Value.ClientPassword,
            Scope = scope
            });

            if (tokenResponse.IsError)
            {
                _logger.LogError($"Unable to get token. Error is:{tokenResponse.Error}");
                throw new Exception("Unable to get token", tokenResponse.Exception);
            }

            return tokenResponse;
        }
    }
}

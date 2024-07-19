using IdentityModel.Client;

namespace MvcClient.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope">Scope we're asking for</param>
        /// <returns></returns>
        Task<TokenResponse> GetTokenAsync(string scope);
    }
}

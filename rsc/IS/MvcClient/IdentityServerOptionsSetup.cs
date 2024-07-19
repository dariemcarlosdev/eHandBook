using Microsoft.Extensions.Options;
using MvcClient.Services;

internal class IdentityServerOptionsSetup : IConfigureOptions<IdentityServerOptions>
{
    private const string SectionName = "IdentityServerSettings";
    private readonly IConfiguration _configuration;

    public IdentityServerOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(IdentityServerOptions options)
    {
        _configuration
            .GetSection(SectionName)
            .Bind(options);
    }
}
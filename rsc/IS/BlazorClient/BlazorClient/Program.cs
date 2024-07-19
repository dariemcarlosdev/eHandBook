using Api;
using BlazorClient.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(); //registering httpclient service

//Configure Options pattern to extract appsettings.json IdentityServerSettings, into IdentityServerOptions instance type,
//and provide strongly typed application settings.
//builder.Services.Configure<IdentityServerOptions>(builder.Configuration.GetSection("IdentityServerSettings"));

//Registering ITokenService in our DI container.
builder.Services.AddSingleton<ITokenService, TokenService>();

// adding support for OpenID Connect authentication to the MVC application
// ref: https://identityserver4.readthedocs.io/en/latest/quickstarts/2_interactive_aspnetcore.html
//JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
//To add support for OpenID Connect authentication to the MVC application
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; //cookie name tha's going to be used to mantain client's auth state.

    //so we're going to use OIDC as the challenge mechanism for users, and what this means is that we're are going to call off into identity server
    //and get identity server to do the user authentication for us.
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme
    , options =>
    {
        options.SlidingExpiration = true;
    }
    )
    //here we set configuration for the Open Id connect server we are going to use.
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
    options =>
    {
        //options.Authority = "https://localhost:5001"; //Identity Server URL Address
        //options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
        //options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
        //options.ClientId = "interactive.client";
        //options.ClientSecret = "secret";

        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
        // Set Authority to setting in appsettings.json.  This is the URL of the IdentityServer4  
        options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
        // Set ClientId to setting in appsettings.json.    This Client ID is set when registering the lazor Server app in IdentityServer4  
        options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
        // Set ClientSecret to setting in appsettings.json.  The secret value is set from the Client >  Basic tab in IdentityServer Admin UI  
        options.ClientSecret = builder.Configuration["InteractiveServiceSettings:secret"];
        ////Login  
        //options.CallbackPath = builder.Configuration["InteractiveServiceSettings:RedirectUri"];
        ////Logout  
        //options.SignedOutCallbackPath = builder.Configuration["InteractiveServiceSettings:PostLogoutRedirectUri"];
        

        //these three options specify the flow when we are talking to identity server.This especify we are using authorization code flow.
        options.ResponseType = "code";
        options.UsePkce = true;
        options.ResponseMode = "query";

        // Add request scopes.The scopes are set in the Client >  Basic tab in IdentityServer Admin UI  
            //we specify the scopes we are asking for
        options.Scope.Add("weatherapi.read");

        //Scope for accessing API
        options.Scope.Add("identityApi");

        // Scope for custom user claim
        options.Scope.Add("appUser_claim");

        // map custom user claim 
        options.ClaimActions.MapUniqueJsonKey("appUser_claim", "appUser_claim");

        //these makes possible that identity and access tokens to be saved, make them available to another part of our code, eg: if we call httpContext.GetTokenAsync().
        options.SaveTokens = true;
        // It's recommended to always get claims from the   
        // UserInfoEndpoint during the flow.  
        options.GetClaimsFromUserInfoEndpoint = true;
        
    });

builder.Services.AddAuthorization(authorizationOptions =>
{
    // add authorization poliy from shared project. This is the same policy used by the API
    authorizationOptions.AddPolicy(
        Policies.CanViewIdentity,
        Policies.CanViewIdentityPolicy());
});



IdentityModelEventSource.ShowPII = true; //Add this line

builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

//Add services to the request pipeline in correct processing order:

//UseStaticFiles
//UseRouting
//UseAuthentication
//UseAuthorization
//UseEndpoints

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

//adding autho. and authent capabilities.
// add authentication first, followed by authorization
// these should come after app.UseRouting and before app.UseEndpoints
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");

});


app.Run();

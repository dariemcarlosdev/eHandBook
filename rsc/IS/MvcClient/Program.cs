using Microsoft.IdentityModel.Logging;
using MvcClient.Services;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;

// adding support for OpenID Connect authentication to the MVC application
// ref: https://identityserver4.readthedocs.io/en/latest/quickstarts/2_interactive_aspnetcore.html
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
//To add support for OpenID Connect authentication to the MVC application
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies"; //cookie name tha's going to be used to mantain client's auth state.

    //so we're going to use OIDC as the challenge mechanism for users, and what this means is that we're are going to call off into identity server
    //and get identity server to do the user authentication for us.
    options.DefaultChallengeScheme = "oidc"; 
})
    .AddCookie("Cookies"
    //, options => 
    //{   
    //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    //}
    )
    //here we set configuration for the Open Id connect server we are going to use.
    .AddOpenIdConnect("oidc", options =>
    {
        //options.Authority = "https://localhost:5001"; //Identity Server URL Address
        //options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
        //options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
        //options.ClientId = "interactive.client";
        //options.ClientSecret = "secret";
        options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
        options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
        options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];

        //these three options specify the flow when we are talking to identity server.This especify we are using authorization code flow.
        options.ResponseType = "code";
        options.UsePkce = true;
        options.ResponseMode = "query";

        //we specify the scopes we are asking for
        options.Scope.Add(builder.Configuration["InteractiveServiceSettings:Scopes:0"]);

        //these makes possible that identity and access tokens to be saved, make them available to another part of our code, eg: if we call httpContext.GetTokenAsync().
        options.SaveTokens = true;


    });

// Add services to the container.
builder.Services.AddControllersWithViews();
//Configure Options pattern to extract appsettings.json IdentityServerSettings, into IdentityServerOptions instance, and provide strongly typed application settings.
builder.Services.Configure<IdentityServerOptions>(builder.Configuration.GetSection("IdentityServerSettings")) ;
//here Another way :
//builder.Services.ConfigureOptions<IdentityServerOptionsSetup>();

builder.Services.AddSingleton<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//The RequireAuthorization method disables anonymous access for the entire application.
//You can also use the [Authorize] attribute, if you want to specify authorization on a per controller or action method basis.
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute()
        .RequireAuthorization();
});



app.Run();

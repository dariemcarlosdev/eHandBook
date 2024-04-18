
using Api;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Setting up IdentityServer4 Authorization Within the API.
// accepts any access token issued by identity server
builder.Services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication("Bearer", options =>
            {
                //this options tell this application how to call IS.
                options.Authority = "https://localhost:5001"; //the address that IS is listening on.               
                options.ApiName = "weatherapi"; //specify api name, which is the resource tha's been protected.
                options.ApiName = "identityApi";

// adds authorization policy to make sure the token is for scope 'weatherapi'.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "weatherapi.read");
    });
});

//builder.Services.AddCors(options =>
//{
//    // this defines a CORS policy called "default"
//    options.AddPolicy("default", policy =>
//    {
//        policy.WithOrigins("http://localhost:5100")
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseHttpsRedirection();

app.Run();

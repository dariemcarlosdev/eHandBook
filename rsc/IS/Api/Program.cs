using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// accepts any access token issued by identity server
builder.Services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication("Bearer", options =>
            {
                //this options tell this application how to call IS.
                options.Authority = "https://localhost:5001"; //the address that IS is listening on.
               
                options.ApiName = "weatherapi"; //specify api name, which is the resource tha's been protected.
                //options.TokenValidationParameters = new TokenValidationParameters
                //{
                //    ValidateAudience = false
                //};
            });

// adds an authorization policy to make sure the token is for scope 'weatherapi'
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "weatherapi.read");
    });
});


builder.Services.AddControllers();

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
//app.UseHttpsRedirection();

app.Run();

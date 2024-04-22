using eHandbook.api.EndPoints;
using eHandbook.Infrastructure.Configurations.OptionsPattern;
using eHandbook.Infrastructure.CrossCutting.ExceptionsHandler.Middlewares;
using eHandbook.Infrastructure.CrossCutting.HealthCheck;
using eHandbook.Infrastructure.Extentions;
using eHandbook.Infrastructure.Middlewares;
using eHandbook.modules.ManualManagement.CoreDomain.Validations.FluentValidation;
using eHandbook.modules.ManualManagement.Infrastructure.Extensions;
using eHandbook.modules.ManualManagement.Infrastructure.Persistence;
using eHandbook.modules.ManualManagement.Infrastructure.Persistence.Interceptors;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using NLog;
using Serilog;
using Sieve.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Setting the JSON serializer options
/*
 public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
            .AddJsonOptions(options =>
               options.JsonSerializerOptions.PropertyNamingPolicy = null);
}
 This works well for controllers. .NET 6 introduced minimal APIs for hosting and routing in web applications. This is an easier way to create small web APIs. 
This new model does not use controllers, so you cannot use the AddJsonOptions method. AddJsonOptions configures Microsoft.AspNetCore.Http.Json.JsonOptions using the Dependency Injection (source).
So, you can do the same directly.In the following example, I configure the JSON serializer options using the Configure method:
 */
//builder.Services.ConfigureOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>(); Test if this also work.
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{

    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.WriteIndented = true;

});

//With AddValidatorsFromAssembly(),all the validators defined in the executing assembly will be automatically registered, eliminating the need to manually register each validator. This approach ensures that all validators are available for request validation within our project.
//This cross-cutting concerns can be set in Sharead Infrastructure project.
builder.Services.AddValidatorsFromAssemblyContaining<GetManualByIdReqQueryValidator>(ServiceLifetime.Singleton);
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


//Initialize Services Collection for Manual Module and shared Infrastructure DI Container Service Collection.
builder.Services
    .AddManualModuleDIServiceCollection()
    .AddSharedInfraDIServiceCollection();

//Add Entity framework services using Option Pattern.
builder.Services.ConfigureOptions<DataBaseOptionsSetUp>();

//Probably to improve app arquitecture it needs to be include in shared Infrastructure.
//This cross-cutting concerns can be set in Sharead Infrastructure project.
builder.Services.AddSingleton<UpdateMyAuditableEntitiesInterceptor>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Configuring Sieve package for using config set in appsettings.json

builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));

// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddSwaggerGen(c =>
{
    //Configure annotations in Swagger documentation
    c.EnableAnnotations();
    //c.IncludeXmlComments(string.Format(@"{0}\EFCore.CodeFirst.WebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
    c.SwaggerDoc("V1", new OpenApiInfo
    {
        Title = "eHandBook API V1",
        Version = "V1.0",
        Description = "An API to perform Manuals Management operations",
        TermsOfService = new Uri("https://includeaurlfortermofservice.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Dariem Macias Mora",
            Email = "335286@dadeschools.net",
            Url = new Uri("https://includeaurltocontact.com/"),
        },
        License = new OpenApiLicense
        {
            Name = "eHandBook API LICX",
            Url = new Uri("https://ifapplicenedincludehere.com/license"),
        }
    });
    c.SwaggerDoc("V2", new OpenApiInfo
    {
        Title = "eHandBook API V2",
        Version = "V2.0",
        Description = "An API to perform Manuals Management operations",
        TermsOfService = new Uri("https://includeaurlfortermofservice.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Dariem Macias Mora",
            Email = "335286@dadeschools.net",
            Url = new Uri("https://includeaurltocontact.com/"),
        },
        License = new OpenApiLicense
        {
            Name = "eHandBook API LICX",
            Url = new Uri("https://ifapplicenedincludehere.com/license"),
        }
    });
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"SwaggerAnnotation.xml"; //name defined in Project Setting > Output > XML Documentation file path.
    var xmlPath = Path.Combine(System.AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//Configuring and use Serilog for Logging thru AppicationSettings.json by accessing the configure host Builder instance and provide delegate to configure Serilog/ 
// Overload to use is HostbuilderContext and loggerConfiguration intances.by default Serilog look for Serilog section in configuration
builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration));

//Configuring and use NLog and loading everything from NLog.config instead of appsettings.json.
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

//registers the health check services.
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ManualDbContext>()
    .AddCheck<DatabaseServiceCustomHealthCheck>("SQLServer");


WebApplication app = builder.Build();



// Configure the HTTP request pipeline. Adding middlewares.

//In the bellow code, the Serilog request logging middleware is added to log HTTP request information.
//The GetLevel function is used to determine the log level based on the HTTP context, elapsed time, and any exceptions.
app.UseSerilogRequestLogging(options =>
{
    options.GetLevel = (ctx, elapsed, ex) =>
    {
        if (ex != null || ctx.Response.StatusCode > 499)
            return Serilog.Events.LogEventLevel.Error;
        if (elapsed > TimeSpan.FromSeconds(3).TotalSeconds)
            return Serilog.Events.LogEventLevel.Warning;
        return Serilog.Events.LogEventLevel.Information;
    };
});

if (app.Environment.IsDevelopment())
{
    app
   //Seeding database Commented out for increase app startup time.     
   //.SeedSqlServer()

   // Enable middleware to serve generated Swagger as a JSON endpoint.    
   .UseSwagger()
   // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
   // specifying the Swagger JSON endpoint.
   .UseSwaggerUI(options =>
   {
       options.SwaggerEndpoint("/swagger/V1/swagger.json", "V1.0");
       options.SwaggerEndpoint("/swagger/V2/swagger.json", "V2.0");

   });

    app.UseMiddleware<GlobalExceptionErrorHandlerMiddleware>()
    .UseTiming();
}

else
{
    app.UseExceptionHandler("/Error")

    //security feauture
    //This method tell a browser when we return a response to use HTTPS 
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    .UseHsts()

    //Cors Policy defination. Cross-Origin Resource Sharing (CORS) is an HTTP-header based mechanism that allows a server to indicate any origins (domain, scheme, or port) other than its own from which a browser should permit loading resources.
    //for now we are allowing all origins
    .UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}




//enable static file middleware for using wwwroot with static files.
app.UseStaticFiles()

.UseHttpsRedirection();

//add healthCheck middleware to response at the specified URL with Formatting Health Checks Response.
app.MapHealthChecks(
    "/_appHealthCheck",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    })
    //--- For Limiting who can call this endpoint.Leak of implementation.
    /*.RequireAuthorization().RequireHost().RequireCors()*/;



//First approach.Global error Handler Defining Middleware
//app.Use(async(context, next) =>
//{
//    //every request is going to be execute inside of our try catch logic
//    try
//    {
//        await next(context);
//    }
//    catch (Exception e)
//    {
//        //having access to http response of this request.
//        //whenever we catch an unhanddled exception in the middleware we set status code to 500(internal server error);
//        context.Response.StatusCode = 500; 
//    }

//});


//Minimal APIs for Manual Service call.
app.MapManualEndPoints();


app.Run();

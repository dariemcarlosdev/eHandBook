using eHandbook.Core.Domain.Abstractions;
using eHandbook.Core.Persistence.Abstractions;
using eHandbook.Core.Persistence.Repositories.Common;
using eHandbook.Infrastructure.Abstractions.Loggin;
using eHandbook.Infrastructure.Configurations.OptionsPattern;
using eHandbook.Infrastructure.CrossCutting.Logging;
using eHandbook.modules.ManualManagement.Application.Abstractions;
using eHandbook.modules.ManualManagement.Application.Services;
using eHandbook.modules.ManualManagement.CoreDomain.EntitiesModels;
using eHandbook.modules.ManualManagement.Infrastructure.Configuration.FluentAPIs;
using eHandbook.modules.ManualManagement.Infrastructure.Persistence;
using eHandbook.modules.ManualManagement.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sieve.Services;

namespace eHandbook.modules.ManualManagement.Infrastructure.Extensions
{
    public static class ManualModuleDependencyInjections
    {
        //This Class can also access to the SharedInfrastructureServiceCollectionExtentions Class methods to extend Manual Module ServiceCollection methods.

        /// <summary>
        /// Manual Module Initializar Service extension Method.
        /// Three ways to register Service:
        /// * By calling builder.Services.AddSingleton will create the service the first time you request it and then every subsequent request is calling the same instance of the service.
        ///   This means that all components are sharing the same service every time they need it.You are using the same instance always
        /// * By calling builder.Services.AddScoped will create the service once per request.That means whenever we send the HTTP request towards the application,
        ///   a new instance of the service is created
        /// * By calling builder.Services.AddTransient will create the service each time the application request it.
        ///   This means that if during one request towards our application, multiple components need the service, this service will be created again for every single component which needs it
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>

        public static IServiceCollection ManualModuleDIRegistrations(this IServiceCollection services)

        {
            /* Services lifetime:
            Transient objects are always different; a new instance is provided to every controller and every service.
            Scoped objects are the same within a request, but different across different requests
            Singleton objects are the same for every object and every request(regardless of whether an instance is provided in ConfigureServices)
            */


            //Registering Interceptor service.
            services.AddSingleton<UpdateMyAuditableEntitiesInterceptor>()

            .AddSingleton<IAuditableEntity, ManualEntity>()

            //inject Service layer  inside  Manual Module the .NET Core’s IOC container
            .AddScoped<IManualService, ManualServices>()

            //Injecting/registering MyCustomSieveProcessor to take adavantage of Dependency Injection. The ISieveProcessor interface to resolve to our CustomSieveProcessor implementation.
            //pkg ref:https://github.com/Biarity/Sieve
            .AddSingleton<ISieveProcessor, MyCustomSieveProcessor>()

            //Adding the logger service  inside Manual Module the .NET Core’s IOC container
            .AddScoped<ILoggerManager, LoggerManager>()
            //Implementation here.

            //services.AddTransient<IMapper, ManualMapper>();

            //BugUpdatingClassInstanceCannotBeTracked solved.This error indicates there are two different Db context instances. This error might be related to how the service scope is configured but this is only a guess since the community cannot see the relevant code.
            //The solution was change the class lifetime instance to Scoped, it was Singleton.
            .AddScoped<DbContext, ManualDbContext>()
            //Registering DI using generics types.
            //https://stackoverflow.com/questions/56271832/how-to-register-dependency-injection-with-generic-types-net-core
            .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))

            //Use DbContext Pooling.
            /*
            Important here: A DbContext is often a lightweight object: creating and disposing of one does not require a database activity, and most applications may do so with little to no performance effect. However, each context instance builds up various internal services and objects required for executing its functions, and the overhead of doing so repeatedly might be detrimental to the application’s performance. 
            Here’s exactly where DbContext pooling helps.When a newly created instance is requested, it is returned from the pool rather than being created from scratch. With context pooling, context setup costs are only incurred once at the program startup time rather than every time the application runs. When using context pooling, EF Core resets the state of the context instance and places it in the pool when you dispose of an instance.
            You can leverage the built-in support for DbContext pooling in EF Core to enhance performance. Using this feature, you can reuse previously generated DbContext instances rather than building them repeatedly. DbContext pooling, in other words, enables you to reuse pre-created instances to achieve a speed benefit.
             */
            .AddDbContextPool<ManualDbContext>(
                (serviceProvider, options) =>
                {   //Register Interceptor and inject to DbContext configurations
                    var auditableInterceptor = serviceProvider.GetService<UpdateMyAuditableEntitiesInterceptor>()!;
                    var databaseOptions = serviceProvider.GetService<IOptions<DataBaseOptions>>()!.Value;

                    options.UseSqlServer(databaseOptions.ConnectionString, SqlServerAction =>
                    {
                        SqlServerAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
                        SqlServerAction.CommandTimeout(databaseOptions.CommandTimeOut);

                        //match our EF migrations assembly eHandbook.modules.ManualManagement to be loaded.
                        SqlServerAction.MigrationsAssembly("eHandbook.modules.ManualManagement");
                    }).AddInterceptors(auditableInterceptor);
                    options.EnableDetailedErrors(true);
                    options.EnableSensitiveDataLogging(true);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                });

            // After migration:
            //use of the FluentValidation.DependencyInjectionExtensions package which can be used to automatically find and register all the validators in a specific assembly
            //using extension method AddValidatorsFromAssemblyContaining.By default, these will be registered as Scoped, but you can optionally use Singleton or Transient instead:
            //e.g services.AddValidatorsFromAssemblyContaining<AnyValidator>(ServiceLifetime.Transient);

            //not bein registered here.
            //.AddValidatorsFromAssemblyContaining(typeof(RequestManualValidator));//Singleton class lifetime ensures that only one instance of a service is created and shared throughout the application's lifetime.

            //to exclude some validators from automatic registration.For example, to register all validators except the CustomerValidator you could write the following:
            //services.AddValidatorsFromAssemblyContaining<MyValidator>(ServiceLifetime.Scoped, filter => filter.ValidatorType != typeof(CustomerValidator));

            return services;
        }

        /// <summary>
        /// Extension method to start all the migrations at the application’s startup.
        /// </summary>
        /// <param name="app">WebApplication type</param>
        /// <returns>WebApplication type</returns>
        /// <exception cref="Exception"></exception>
        public static WebApplication SeedSqlServer(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using (var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<ManualDbContext>();
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return app;
        }



    }
}

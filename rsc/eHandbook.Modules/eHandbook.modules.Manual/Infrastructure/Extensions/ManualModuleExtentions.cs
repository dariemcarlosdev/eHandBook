using eHandbook.Core.Domain.Common;
using eHandbook.Core.Persistence.Repositories.Common;
using eHandbook.Infrastructure.Logging;
using eHandbook.Infrastructure.Logging.Contracts;
using eHandbook.Infrastructure.Options;
using eHandbook.modules.ManualManagement.Application.Contracts;
using eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManual;
using eHandbook.modules.ManualManagement.Application.Service;
using eHandbook.modules.ManualManagement.CoreDomain.Entities;
using eHandbook.modules.ManualManagement.CoreDomain.Validations;
using eHandbook.modules.ManualManagement.Infrastructure.Persistence;
using eHandbook.modules.ManualManagement.Infrastructure.Persistence.Interceptors;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace eHandbook.modules.ManualManagement.Infrastructure.Extensions
{
    public static class ManualModuleExtentions
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
        
        public static IServiceCollection AddManualModuleServiceCollection(this IServiceCollection services)

        {
            /* Services lifetime:
            Transient objects are always different; a new instance is provided to every controller and every service.
            Scoped objects are the same within a request, but different across different requests
            Singleton objects are the same for every object and every request(regardless of whether an instance is provided in ConfigureServices)
            */


            //Registering Interceptor service.
            services.AddSingleton<UpdateMyAuditableEntitiesInterceptor>();

            services.AddSingleton<IAuditableEntity, ManualEntity>();

            //inject Service layer  inside  Manual Module the .NET Core’s IOC container
            services.AddScoped<IManualService, ManualServices>();

            //Adding the logger service  inside Manual Module the .NET Core’s IOC container
            services.AddScoped<ILoggerManager, LoggerManager>();
            //Implementation here.

            //services.AddTransient<IMapper, ManualMapper>();
            services.AddSingleton<DbContext, ManualDbContext>();
            //Registering DI using generics types.
            //https://stackoverflow.com/questions/56271832/how-to-register-dependency-injection-with-generic-types-net-core
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            //register MediatR and provide default configuration to the constructor.
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(GetManualByIdQuery)));

            //Use DbContext Pooling.
            /*
            Important here: A DbContext is often a lightweight object: creating and disposing of one does not require a database activity, and most applications may do so with little to no performance effect. However, each context instance builds up various internal services and objects required for executing its functions, and the overhead of doing so repeatedly might be detrimental to the application’s performance. 
            Here’s exactly where DbContext pooling helps.When a newly created instance is requested, it is returned from the pool rather than being created from scratch. With context pooling, context setup costs are only incurred once at the program startup time rather than every time the application runs. When using context pooling, EF Core resets the state of the context instance and places it in the pool when you dispose of an instance.
            You can leverage the built-in support for DbContext pooling in EF Core to enhance performance. Using this feature, you can reuse previously generated DbContext instances rather than building them repeatedly. DbContext pooling, in other words, enables you to reuse pre-created instances to achieve a speed benefit.
             */
            services.AddDbContextPool<ManualDbContext>(
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

            // Before
            //Depredicated
            //services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ManualEntityValidator>());
            
            // After migration:
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<GetManualByIdRequestValidator>();

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
                using (var context = scope.ServiceProvider.GetRequiredService<ManualDbContext>())
                {
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return app;
        }



    }
}

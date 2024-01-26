using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using eHandbook.modules.ManualManagement.Application.Service;
using eHandbook.modules.ManualManagement.Application.Contracts;
using eHandbook.modules.ManualManagement.Infrastructure.Persistence;
using eHandbook.Infrastructure.CrossCutting.Logging;
using eHandbook.Infrastructure.CrossCutting.Logging.Contracts;

namespace eHandbook.modules.ManualManagement.Extensions
{
    public static class ManualServiceCollectionExtentions
    {

        //This Class can also access to the SharedInfrastructureServiceCollectionExtentions Class methods to extend Manual Module ServiceCollection methods.


        /// <summary>
        /// Manual Module Initializar Service extension Method.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddManualModuleInitializer(this IServiceCollection services)

        {
            /*
             Three ways to register Service:
            
            * By calling builder.Services.AddSingleton will create the service the first time you request it and then every subsequent request is calling the same instance of the service. 
            This means that all components are sharing the same service every time they need it. You are using the same instance always
            
            * By calling builder.Services.AddScoped will create the service once per request. That means whenever we send the HTTP request towards the application,
            a new instance of the service is created
            
            * By calling builder.Services.AddTransient will create the service each time the application request it.
            This means that if during one request towards our application, multiple components need the service, this service will be created again for every single component which needs it
             */

            //inject Data Access Layer - Repository inside the Manual Module .NET Core’s IOC container
            //services.AddScoped<IManualRepository, ManualRepository>();

            //inject Service layer  inside the Manual Module the .NET Core’s IOC container
            services.AddScoped<IManualService, ManualServices>();


            //Adding the logger service  inside the Manual Module the .NET Core’s IOC container
            services.AddSingleton<ILoggerManager, LoggerManager>();
            //Implementation here.
            return services;
        }

        /// <summary>
        /// Extension method to start all the migrations at the application’s startup.
        /// </summary>
        /// <param name="app">WebApplication type</param>
        /// <returns>WebApplication type</returns>
        /// <exception cref="Exception"></exception>
        public static WebApplication UseItToSeedSqlServer(this WebApplication app)
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

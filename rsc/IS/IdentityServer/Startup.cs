// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public IWebHostEnvironment Environment { get; set; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
            // see https://identityserver4.readthedocs.io/en/latest/quickstarts/2_interactive_aspnetcore.html
            //Creating instance of Identity Server and adding in-memory support for clients, resources, scopes and test users.

            //services.AddIdentityServer()
            //            .AddInMemoryIdentityResources(Config.IdentityResources)
            //            .AddInMemoryApiResources(Config.ApiResources)
            //            .AddInMemoryApiScopes(Config.ApiScopes)
            //            .AddInMemoryClients(Config.Clients)
            //            The sample UI also comes with an in-memory “user database”. You can enable this in IdentityServer by adding the AddTestUsers extension method
            //            .AddTestUsers(TestUsers.Users)
            //             not recommended for production - you need to store your key material somewhere secure
            //             we do this cuz things in the tokens that IS delivers for us, need to be signed, and this AddDeveloperSigningCredential() provides that signing materiual for us.
            //            https://identityserver4.readthedocs.io/en/latest/topics/startup.html#key-material
            //            .AddDeveloperSigningCredential();

            //Adding EF Support to Identity Server.
            //https://identityserver4.readthedocs.io/en/aspnetcore2/quickstarts/7_entity_framework.html
            //var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var connectionString2 = Configuration.GetConnectionString("DefaultConnection2");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<AspNetIdentityDbContext>(options =>
                options.UseSqlServer(connectionString2,
                    sql => sql.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetIdentityDbContext>()
                .AddDefaultTokenProviders();




            var builder = services.AddIdentityServer(options =>
           {

               options.Events.RaiseErrorEvents = true;
               options.Events.RaiseInformationEvents = true;
               options.Events.RaiseFailureEvents = true;
               options.Events.RaiseSuccessEvents = true;

               // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
               options.EmitStaticAudienceClaim = true;
           })
           //The sample UI also comes with an in-memory “user database”. You can enable this in IdentityServer by adding the AddTestUsers extension method
           //.AddTestUsers(TestUsers.Users)
           .AddAspNetIdentity<IdentityUser>()
           // this adds the config data from DB (clients, resources)
           //The configuration data is the configuration information about resources and clients.
           .AddConfigurationStore(options =>
           {
               options.ConfigureDbContext = builder =>
                builder.UseSqlServer(connectionString2,
                   sql => sql.MigrationsAssembly(migrationsAssembly));
           })
           //The operational data is information that IdentityServer produces as it’s being used such as tokens, codes, and consents.
           .AddOperationalStore(options =>
           {
               options.ConfigureDbContext = builder =>
                   builder.UseSqlServer(connectionString2,
                    sql => sql.MigrationsAssembly(migrationsAssembly));

               // this enables automatic token cleanup. this is optional.
               options.EnableTokenCleanup = true;
               options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
           });
            // not recommended for production - you need to store your key material somewhere secure
            // we do this cuz things in the tokens that IS delivers for us, need to be signed, and this AddDeveloperSigningCredential() provides that signing materiual for us.
            //https://identityserver4.readthedocs.io/en/latest/topics/startup.html#key-material
            builder.AddDeveloperSigningCredential();


        }

        //private void InitializeDatabase(IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        //    {
        //        serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

        //        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        //        context.Database.Migrate();
        //        if (!context.Clients.Any())
        //        {
        //            foreach (var client in Config.Clients.ToList())
        //            {
        //                context.Clients.Add(client.ToEntity());
        //            }
        //            context.SaveChanges();
        //        }

        //        if (!context.IdentityResources.Any())
        //        {
        //            foreach (var resource in Config.IdentityResources.ToList())
        //            {
        //                context.IdentityResources.Add(resource.ToEntity());
        //            }
        //            context.SaveChanges();
        //        }

        //        if (!context.ApiResources.Any())
        //        {
        //            foreach (var resource in Config.ApiResources.ToList())
        //            {
        //                context.ApiResources.Add(resource.ToEntity());
        //            }
        //            context.SaveChanges();
        //        }
        //    
        //}




        public void Configure(IApplicationBuilder app)
        {


            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }



            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();

            //uncomment, if you want to anable MVC Authorization
            //otherwise we won't be able to properly authenticate the client app.
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

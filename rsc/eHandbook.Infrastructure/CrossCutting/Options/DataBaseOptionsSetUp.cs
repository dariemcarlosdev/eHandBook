using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace eHandbook.Infrastructure.CrossCutting.Options
{
    /// <summary>
    /// Class inherit from IConfigureOptions, that is resolved inside the ASP.NET Core at the runtime, which means we have access to DI inside of this class.
    /// </summary>
    public class DataBaseOptionsSetUp : IConfigureOptions<DataBaseOptions>
    {
        //private constant string containing name of configuration section

        private const string AppSettingJsonConfigSectionName = "DataBaseOptionsPattern";

        //private property to access to application configuration properties.
        //IConfiguration instance which is the configuration root object. As of .NET Core 2.0 IConfiguration is a default service that can get injected automatically
        //- it's one of the pre-configured services registered with the DI system as part of the .NET Core bootstrapping process.
        private readonly IConfiguration configuration;


        // new .net 8 constructor parameter definition.
        public DataBaseOptionsSetUp(IConfiguration configuration) => this.configuration = configuration;


        public void Configure(DataBaseOptions options)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;

            //create a Database options instance to configure. Database options will be resolve only once first time it's injected somewhere, so that its value cannot be changed at the runtime
            //by changing configuration in appsettings.json, so to apply new changes to Database options is update appsettings.json and restart app.
            options.ConnectionString = connectionString;


            //you can easily bind a configuration instance(or interface) explicitly without having to go through the IOptions<T> interface.
            var section = configuration.GetSection(AppSettingJsonConfigSectionName);
            section.Bind(options);
        }

    }

}

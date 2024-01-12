namespace eHandbook.Infrastructure.Options
{
    /// <summary>
    /// Used to store all the options we want to pulling up from appsettings.{environment}.json configuration file.
    /// </summary>
    public class DataBaseOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int MaxRetryCount { get; set; }
        public int CommandTimeOut { get; set; }
        public bool EnableDetailedErrors { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }

    }
}

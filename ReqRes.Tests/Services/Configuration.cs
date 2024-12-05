using Microsoft.Extensions.Configuration;

namespace Services
{
    public static class Configuration
    {
        private static readonly IConfigurationRoot _config;

        static Configuration()
        {
            var configFile = Directory.GetFiles(Directory.GetCurrentDirectory(), "appsettings.*.json").First();

            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                .Build();
        }

        private static string Get(string key)
        {
            return _config[key];
        }

        public static string BaseUrl => Get("BaseUrl");
    }
}

using EBookStore.Interfaces;

namespace EBookStore.Implementations
{
    public class ConfigurationAccessor : IConfigurationAccessor
    {
        private readonly IConfiguration _configuration;

        public ConfigurationAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T GetValue<T>(string key)
        {
            return _configuration.GetValue<T>(key);
        }
    }
}

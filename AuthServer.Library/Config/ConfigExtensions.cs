using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthServer.Library.Config
{
    public static class ConfigExtensions
    {
        public static IServiceCollection AddConfiguration<TInterface, TClass>(this IServiceCollection services, IConfiguration configuration)
            where TInterface : class
            where TClass : class, TInterface, new()
        {
            var config = configuration
                .GetSection(typeof(TClass).Name)
                .Get<TClass>();

            return services
                .AddSingleton<TInterface>(config)
                .AddSingleton<TClass>(config);
        }
    }
}

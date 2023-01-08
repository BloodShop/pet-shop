using System.Reflection;

namespace PetShopProj.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection InstallerServices(
            this IServiceCollection services,
            IConfiguration configuration,
            string environmentName = "",
            params Assembly[] assemblies)
        {
            IEnumerable<IServiceInstaller> serviceInstallers = assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(IsAssignableToType<IServiceInstaller>)
                .Select(Activator.CreateInstance)
                .Cast<IServiceInstaller>();

            foreach (var serviceInstaller in serviceInstallers)
                serviceInstaller.Install(services, configuration, environmentName);

            return services;

            static bool IsAssignableToType<T>(TypeInfo typeInfo) => 
                typeof(T).IsAssignableFrom(typeInfo) &&
                !typeInfo.IsInterface &&
                !typeInfo.IsAbstract;
        }
    }
}

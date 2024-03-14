using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Kernel.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInstallersFromAssemblyContaining<T>(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInstallersFromAssemblyContaining(configuration, typeof(T));
        }

        public static void AddInstallersFromAssemblyContaining(this IServiceCollection services, IConfiguration configuration, params Type[] assemblyMarkers)
        {
            var assemblies = assemblyMarkers.Select(x => x.Assembly).ToArray();
            services.AddInstallersFromAssemblies(configuration, assemblies);
        }

        public static void AddInstallersFromAssemblies(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var installerTypes = assembly.DefinedTypes.Where(x =>
                    typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

                var installers = installerTypes.Select(Activator.CreateInstance).Cast<IInstaller>();

                foreach (var installer in installers.OrderBy(x => x.Order))
                {
                    installer.InstallServices(configuration, services);
                }
            }
        }
    }
}

using Kernel.Messages;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Messaging.PubSub.DependencyInjection
{
    public static class AddMessagesContractsExtension
    {
        public static void AddMessagesContractsFromAssemblyContaining<T>(this IServiceCollection services)
        {
            services.AddMessagesContractsFromAssemblyContaining(typeof(T));
        }

        public static void AddMessagesContractsFromAssemblyContaining(this IServiceCollection services, params Type[] assemblyMarkers)
        {
            var assemblies = assemblyMarkers.Select(x => x.Assembly).ToArray();
            services.AddMessagesContractsFromAssemblies(assemblies);
        }

        public static void AddMessagesContractsFromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            var result = services.Scan(scan => scan.FromAssemblies(assemblies)
                .AddClasses(classes =>
                    classes.AssignableTo<IBaseNotification>()
                ).AsImplementedInterfaces());
        }
    }
}

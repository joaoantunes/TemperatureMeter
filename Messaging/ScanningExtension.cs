using Kernel.Messages;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Messaging
{
    public static class ScanningExtension
    {
        public static IServiceCollection ScanContracts(this IServiceCollection services)
        {
            var result = services.Scan(scan => scan.FromAssemblies(Assembly.GetEntryAssembly())
            .AddClasses(classes =>
                classes.AssignableTo<IBaseMessage>()
            ).AsImplementedInterfaces());
            return result;
        }

    }
}

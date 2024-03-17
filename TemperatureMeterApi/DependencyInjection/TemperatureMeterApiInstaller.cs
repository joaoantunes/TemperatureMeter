using Kernel.DependencyInjection;

namespace TemperatureMeterApi.DependencyInjection
{
    public class TemperatureMeterApiInstaller : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            //Can add DI here
        }

        public int Order { get; } = -1;
    }
}

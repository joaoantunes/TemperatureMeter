using Kernel.DependencyInjection;

namespace TemperatureMeterApi.DependencyInjection
{
    public class TemperatureMeterApiInstaller : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            //services.AddAutoMapper(typeof(IFontsApiAssemblyMarker));
        }

        public int Order { get; } = -1;
    }
}

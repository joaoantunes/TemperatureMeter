using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kernel.DependencyInjection
{
    public interface IInstaller
    {
        void InstallServices(IConfiguration configuration, IServiceCollection services);
        public int Order => -1;
    }
}

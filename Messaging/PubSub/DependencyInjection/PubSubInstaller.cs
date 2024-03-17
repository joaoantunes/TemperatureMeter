using Kernel.DependencyInjection;
using Messaging.PubSub.Publishers;
using Messaging.PubSub.Subscribers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Messaging.PubSub.DependencyInjection
{
    public class PubSubInstaller : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())); // TODO check if needs to be here
            services.TryAddSingleton<IMqttClientFactory, MqttClientFactory>();
           
            services.TryAddSingleton(c =>
            {
                return new Func<string, int, string, MessageSubscriber>((host, port, channel) =>
                    new MessageSubscriber(host, port, channel, c.GetRequiredService<IMqttClientFactory>(), c.GetRequiredService<IDispatcher>()));
            });

            services.TryAddSingleton(c =>
            {
                return new Func<string, int, MessagePublisher>((host, port) =>
                    new MessagePublisher(host, port, c.GetRequiredService<IMqttClientFactory>()));
            });

            services.TryAddTransient<IDispatcher, Dispatcher>();
            services.TryAddSingleton<IMessagePublisherFactory, MessagePublisherFactory>();
            services.TryAddSingleton<IMessageSubscriberFactory, MessageSubscriberFactory>();
        }

        public int Order { get; } = -1;
    }
}

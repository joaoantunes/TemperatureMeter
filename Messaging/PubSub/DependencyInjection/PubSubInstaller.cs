using Kernel.DependencyInjection;
using Kernel.Messages;
using MediatR;
using Messaging.PubSub.Publishers;
using Messaging.PubSub.Subscribers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;
using System.Threading.Channels;

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

            //, c.GetRequiredService<IPublisher>()
            //c.GetServices<IBaseNotification>()
            services.TryAddTransient<IDispatcher, Dispatcher>();

            //services.TryAddSingleton<Func<string, int, string, MessageSubscriber>>(c =>
            //new Func<string, int, string, MessageSubscriber>((host, port, channel) =>
            //    new MessageSubscriber(host, port, channel, c.GetRequiredService<IMqttClientFactory>(), c.GetRequiredService<IEnumerable<BaseMessage>>())));

            services.TryAddSingleton<IMessagePublisherFactory, MessagePublisherFactory>();
            services.TryAddSingleton<IMessageSubscriberFactory, MessageSubscriberFactory>();
        }

        public int Order { get; } = -1;
    }
}

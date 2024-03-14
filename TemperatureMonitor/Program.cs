using Messaging.PubSub.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TemperatureMeter.Domain.Events;
using Kernel.DependencyInjection;
using TemperatureMonitor.ConsoleApp;

IHost host = Host.CreateDefaultBuilder().ConfigureServices(
    (context, services) =>
    {
        services.AddHostedService<MonitorTemperatureBackgroundService>();
        services.AddInstallersFromAssemblyContaining<IPubSubMarker>(context.Configuration);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddMessagesContractsFromAssemblyContaining<TemperatureMeteringCreated>();
    }).UseConsoleLifetime().Build();

host.Run();
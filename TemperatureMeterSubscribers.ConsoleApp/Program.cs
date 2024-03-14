using Kernel.DependencyInjection;
using Messaging;
using Messaging.PubSub.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TemperatureMeterSubscribers.ConsoleApp;

Console.WriteLine("Hello, World!");


IHost host = Host.CreateDefaultBuilder().ConfigureServices(
    (context, services)=>
    {
        //services.AddLogging();
        services.AddHostedService<MyBackgroundService>();
        services.AddInstallersFromAssemblyContaining<IPubSubMarker>(context.Configuration);
        services.ScanContracts();
        
    }).UseConsoleLifetime().Build();

host.Run(); //TODO run async

﻿using Kernel.DependencyInjection;
using Messaging.PubSub.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TemperatureMeter.Domain.Converter;

namespace TemperatureMeter.Application.DependencyInjection
{
    public class TemperatureMeterInstaller : IInstaller
    {
        public int Order => -1;

        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddInstallersFromAssemblyContaining<IPubSubMarker>(configuration);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddTransient<IKelvinToCelsiusConverter, KelvinToCelsiusConverter>();
            // TODO automapper
            //services.AddAutoMapper(typeof(MappingProfile));
            // TODO validators?
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        }
    }
}

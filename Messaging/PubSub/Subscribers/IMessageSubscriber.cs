﻿namespace Messaging.PubSub.Subscribers
{
    public interface IMessageSubscriber : IDisposable
    {
        Task StartAsync();
        Task StopAsync();
    }
}

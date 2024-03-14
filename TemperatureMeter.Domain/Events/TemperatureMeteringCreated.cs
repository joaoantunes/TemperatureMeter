using Kernel.Messages;

namespace TemperatureMeter.Domain.Events
{
    public class TemperatureMeteringCreated : BaseMessage
    {
        public Guid DeviceId { get; set; }
        public double TemperatureInCelcius { get; set; } 
    }
}

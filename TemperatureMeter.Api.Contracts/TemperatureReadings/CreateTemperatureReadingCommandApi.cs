namespace TemperatureMeter.Api.Contracts.TemperatureReadings
{
    public class CreateTemperatureReadingCommandApi
    {
        public Guid DeviceId { get; set; }
        public double TemperatureInKelvin { get; set; }
    }
}

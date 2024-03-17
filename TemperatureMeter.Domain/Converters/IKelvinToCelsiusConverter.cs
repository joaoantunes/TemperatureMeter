namespace TemperatureMeter.Domain.Converters
{
    public interface IKelvinToCelsiusConverter
    {
        double Convert(double temperatureInKelvin);
    }
}

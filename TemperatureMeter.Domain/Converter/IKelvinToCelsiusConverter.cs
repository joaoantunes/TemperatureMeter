namespace TemperatureMeter.Domain.Converter
{
    public interface IKelvinToCelsiusConverter
    {
        double Convert(double temperatureInKelvin);
    }
}

namespace TemperatureMeter.Domain.Converter
{
    public class KelvinToCelsiusConverter : IKelvinToCelsiusConverter
    {
        private double KELVIN_COVERTION_FACTOR = 273.15;
        public double Convert(double temperatureInKelvin)
        {
            //C = K - 273.15 Celcius to Kelvin convertion formula
            double celcius = temperatureInKelvin - KELVIN_COVERTION_FACTOR;
            return celcius;
        }
    }
}

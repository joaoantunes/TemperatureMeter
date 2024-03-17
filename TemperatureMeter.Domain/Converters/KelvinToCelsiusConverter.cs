namespace TemperatureMeter.Domain.Converters
{
    public class KelvinToCelsiusConverter : IKelvinToCelsiusConverter
    {
        private double KELVIN_COVERTION_FACTOR = 273.15;
        public double Convert(double temperatureInKelvin)
        {
            if (temperatureInKelvin < 0)
                throw new ArgumentOutOfRangeException($"Value '{temperatureInKelvin}' can't be lesser than 0, because 0 is the absolute zero");
            //C = K - 273.15 Celcius to Kelvin convertion formula
            double celcius = temperatureInKelvin - KELVIN_COVERTION_FACTOR;
            return celcius;
        }
    }
}

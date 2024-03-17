using TemperatureMeter.Domain.Converters;
using Xunit;

namespace TemperatureMeter.Domain.Tests.Converters
{
    public class KelvinToCelsiusConverterTests
    {
        [Fact]
        public void Convert_GivenValidTemperature_ReturnsExpected() //MethodName_StateUnderTest_ExpectedBehavior
        {
            var converter = new KelvinToCelsiusConverter();
            var originalKelvinTemperature = 0.0;
            var expectedResult = -273.15;

            var actualResult = converter.Convert(originalKelvinTemperature);

            Assert.Equal(expectedResult, actualResult);
        }


        [Theory]
        [InlineData(1, -272.15)]
        [InlineData(273.15, 0)]
        [InlineData(373.15, 100)]
        public void Convert_GivenMultipleValidTemperatures_ReturnsExpected(double inKelvin, double expectedInCelsius)
        {
            var converter = new KelvinToCelsiusConverter();

            var actualResult = converter.Convert(inKelvin);

            Assert.Equal(expectedInCelsius, actualResult);
        }

        [Fact]
        public void Convert_GivenOutOfRangeTemperature_FailsWithArgumentOutOfRangeException()
        {
            var converter = new KelvinToCelsiusConverter();
            var originalKelvinTemperature = -1.0;

            var actualResult = Assert.Throws<ArgumentOutOfRangeException>(() => converter.Convert(originalKelvinTemperature));
            Assert.Contains("can't be lesser than 0", actualResult.Message);
        }
    }
}

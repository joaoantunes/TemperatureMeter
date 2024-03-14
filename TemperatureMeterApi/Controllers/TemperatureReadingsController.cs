using Microsoft.AspNetCore.Mvc;
using TemperatureMeter.Api.Contracts.TemperatureReadings;
using TemperatureMeter.Application.Commands;

namespace TemperatureMeterApi.Controllers
{
    public class TemperatureReadingsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<bool>> CreateReading(CreateTemperatureReadingCommandApi command, CancellationToken cancellationToken)
        {
            // TODO mapping
            // TODO validation
            var request = new CreateTemperatureReadingCommand() { TemperatureInKelvin = command.TemperatureInKelvin };
            var response = await Mediator.Send(request, cancellationToken);
            //var result = Mapper.Map<List<FontResponseApi>>(response);
            bool result = true;
            return Ok(result);
        }
    }
}

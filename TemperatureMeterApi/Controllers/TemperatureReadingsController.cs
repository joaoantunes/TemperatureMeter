using MediatR;
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
            var request = new CreateTemperatureReadingCommand() { TemperatureInKelvin = command.TemperatureInKelvin, DeviceId = command.DeviceId };
            var response = await LazyMediator.Send(request, cancellationToken); // TODO improve error handling and responses
            return Ok(response);
        }
    }
}

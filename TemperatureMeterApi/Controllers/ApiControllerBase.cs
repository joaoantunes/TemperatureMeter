using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TemperatureMeterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Could add v{version:apiVersion} to imply a versioning in API
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
    }
}

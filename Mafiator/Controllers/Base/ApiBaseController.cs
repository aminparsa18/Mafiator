using Microsoft.AspNetCore.Mvc;

namespace Mafiator.Api.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/x-msgpack")]
    [Consumes("application/x-msgpack")]
    public class ApiBaseController : ControllerBase
    {
    }
}
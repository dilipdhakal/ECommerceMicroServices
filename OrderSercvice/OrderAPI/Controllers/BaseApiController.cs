using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderAPI.Controllers
{
    /// <summary>
    /// Base api controller. All controller should extendds this controller.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class BaseApiController : ControllerBase
    {
    }
}

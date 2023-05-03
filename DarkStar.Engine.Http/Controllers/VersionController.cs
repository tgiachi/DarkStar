using Microsoft.AspNetCore.Mvc;

namespace DarkStar.Engine.Http.Controllers;

[Route("version")]
[ApiController]
public class VersionController : ControllerBase
{
    [HttpGet]
    [Route("version")]
    public ActionResult<string> GetVersion() => Ok("1234");
}

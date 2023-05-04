using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace DarkStar.Engine.Http.Controllers;

[Route("api/version")]
[ApiController]
public class VersionController : ControllerBase
{
    [HttpGet]
    [Route("version")]
    public ActionResult<string> GetVersion() => Ok(Assembly.GetExecutingAssembly().GetName().Version.ToString());
}

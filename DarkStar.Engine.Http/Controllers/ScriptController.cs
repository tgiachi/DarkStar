using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Services;

namespace DarkStar.Engine.Http.Controllers;

[Route("api/scripts")]
[ApiController]
public class ScriptController : ControllerBase
{
    private readonly IScriptEngineService _scriptEngineService;
    public ScriptController(IScriptEngineService scriptEngineService) => _scriptEngineService = scriptEngineService;

    [HttpGet]
    [Route("variables")]
    public ActionResult GetVariables() => Ok(_scriptEngineService.ContextVariables);


    [HttpGet]
    [Route("functions")]
    public ActionResult GetFunctions() => Ok(_scriptEngineService.Functions);


    [HttpGet]
    [Route("types")]
    public async Task<IActionResult> GetTypes()
    {
        var scriptDefinition = await _scriptEngineService.GenerateTypeDefinitionsAsync();
        return File(new MemoryStream(Encoding.UTF8.GetBytes(scriptDefinition)), "text/plain", "types.d.ts");
    }
}

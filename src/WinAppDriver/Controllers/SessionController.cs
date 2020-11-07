using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WinAppDriver.Infra;

namespace WinAppDriver.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class SessionController : WinAppDriverControllerBase
  {
    private ISessionManager _sessionManager;

    public SessionController(ISessionManager sessionManager) {
      _sessionManager = sessionManager;
    }

    [HttpPost]
    [Route("")]
    public IActionResult CreateSession([FromBody] object content) {
      return HandleCommand(null, () => _sessionManager.CreateSession(content));
    }

    [HttpDelete]
    [Route("{sessionId}")]
    public IActionResult DeleteSession(string sessionId)
    {
      return HandleCommand(sessionId, () => _sessionManager.DeleteSession(sessionId));
    }

    [HttpPost]
    [Route("{sessionId}/[action]")]
    public IActionResult Back(string sessionId, [FromBody] object content)
    {
      return Ok("Back SessionId " + sessionId + " " + content.ToString());
    }

    [HttpPost]
    [Route("{sessionId}/[action]")]
    public IActionResult Buttondown(string sessionId)
    {
      return Ok("Buttondown SessionId" + sessionId);
    }

    [HttpPost]
    [Route("{sessionId}/[action]")]
    public IActionResult Click(string sessionId)
    {
      return Ok("Click SessionId" + sessionId);
    }

    [HttpPost]
    [Route("{sessionId}/{unknown}", Order = 1)]
    public IActionResult Unknown(string sessionId)
    {
      return Ok("Unknown SessionId " + sessionId);
    }

  }
}

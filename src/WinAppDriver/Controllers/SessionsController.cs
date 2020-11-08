// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using WinAppDriver.Infra;
using WinAppDriver.Infra.CommandHandler;

namespace WinAppDriver.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class SessionsController : WinAppDriverControllerBase
  {
    private ISessionManager _sessionManager;
    private ICommandHandlers _handlers;

    public SessionsController(ISessionManager sessionManager, ICommandHandlers handlers) {
      _sessionManager = sessionManager;
      _handlers = handlers;
    }

    [Route("")]
    [HttpGet]
    public IActionResult GetSessions() {
      var result = _handlers.ExecuteCommand(Command.GetSessions, _sessionManager, null, null, null);
      return ReplyOk(null, result);
    }
  }
}

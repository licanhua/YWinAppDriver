// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using WinAppDriver.Infra.CommandHandler;

namespace WinAppDriver.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class StatusController : WinAppDriverControllerBase
  {
    private ICommandHandlers _handlers;
    public StatusController(ICommandHandlers handlers)
    {
      _handlers = handlers;
    }
    [HttpGet]
    [Route("")]
    public IActionResult Status() 
    {
      return ReplyOk(null, _handlers.ExecuteCommand(Command.Status, null, null, null, null));
    }
  }
}

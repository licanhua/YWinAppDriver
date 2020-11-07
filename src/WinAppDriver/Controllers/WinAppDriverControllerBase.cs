namespace WinAppDriver.Controllers
{
  using Microsoft.AspNetCore.Mvc;
  using System;
  using System.Collections.Generic;
  using WinAppDriver.Infra;

  public abstract class WinAppDriverControllerBase : ControllerBase
  {
    IActionResult ReplyOk(string sessionId, object value)
    {
      return Ok(
        new Dictionary<string, object>() {
          { "sessionId", sessionId },
          { "status", 0 },
          { "value", value }
        }
        );
    }

    protected IActionResult ReplyFail(int httpCode, ResponseStatusCode statusCode, string message = null)
    {
      return StatusCode(httpCode, new Dictionary<string, object>() {
        { "status", (int)statusCode},
        { "value", new Dictionary<string, object>(){
          { "error", statusCode.ToString()},
          { "message", message }}
        }
      });
    }

    protected IActionResult HandleCommand(string sessionId, Action action)
    { 
      return HandleCommand(sessionId, () => { action(); return null; });
    }
    protected IActionResult HandleCommand(string sessionId, Func<object> func)
    {
      try
      {
        return ReplyOk(sessionId, func());
      }
      catch (SessionException ex)
      {
        return ReplyFail(ex.HttpCode, ex.Status, ex.ErrorMessage);
      }
      catch (Exception)
      {
        return ReplyFail(500, ResponseStatusCode.UnknownError, "Unexpected error");
      }
    }
  }
}

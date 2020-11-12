// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WinAppDriver.Infra;
using WinAppDriver.Infra.CommandHandler;
using WinAppDriver.Infra.Communication;
using WinAppDriver.Infra.Result;

namespace WinAppDriver.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class SessionController : WinAppDriverControllerBase
  {
    private ISessionManager _sessionManager;
    private ICommandHandlers _handlers;
    private IElementCommandHandler _elementHandler;

    private IActionResult ExecuteCommand(string sessionId, Func<object> func, Func<string> getSessionIdFunc = null)
    {
      try
      {
        var result = func();
        if (getSessionIdFunc != null)
        {
          sessionId = getSessionIdFunc();
        }
        return ReplyOk(sessionId, result);
      }
      catch (SessionException ex)
      {
        return ReplyFail(ex.HttpCode, ex.Status, ex.ErrorMessage);
      }
      catch (Exception ex)
      {
        return ReplyFail(500, ResponseStatusCode.UnknownError, ex.Message);
      }
    }

    private IActionResult ExecuteCommand(Command command, string sessionId, object body, string elementId)
    {
      return ExecuteCommand(sessionId, () => { return _handlers.ExecuteCommand(command, _sessionManager, sessionId, body, elementId); });
    }

    private IActionResult ExecuteCommand(string sessionId, string elementId, Func<IElement, object> func)
    {
      return ExecuteCommand(sessionId, () => { return _elementHandler.ExecuteCommand(_sessionManager, sessionId, elementId, func); });
    }

    private IActionResult ExecuteCommand(string sessionId, string elementId, Action<IElement> func)
    {
      return ExecuteCommand(sessionId, elementId, (element) => { func(element); return null; });
    }

    public SessionController(ISessionManager sessionManager, ICommandHandlers handlers, IElementCommandHandler elementHandler) {
      _sessionManager = sessionManager;
      _handlers = handlers;
      _elementHandler = elementHandler;
    }

    [HttpPost]
    [Route("")]
    public IActionResult CreateSession([FromBody] object content) {

      string sessionId = null;
      return ExecuteCommand(null, () => {
        var result = (NewSessionIntermediateResult)_handlers.ExecuteCommand(Command.NewSession, _sessionManager, null, content, null);
        sessionId = result.sessionId;
        return result.capabilities;
      }, () => { return sessionId; });

    }

    [HttpPost]
    [Route("{sessionId}/element")]
    public IActionResult FindElement(string sessionId, [FromBody] object content)
    {
      return ExecuteCommand(Command.FindElement, sessionId, content, null);
    }

    [HttpPost]
    [Route("{sessionId}/timeouts")]
    [Route("{sessionId}/timeouts/implicit_wait")]
    public IActionResult SetTimeouts(string sessionId, [FromBody] object content)
    {
      return ExecuteCommand(Command.SetImplicitTimeout, sessionId, content, null);
    }

    [HttpPost]
    [Route("{sessionId}/elements")]
    public IActionResult FindElements(string sessionId, [FromBody] object content)
    {
      return ExecuteCommand(Command.FindElements, sessionId, content, null);
    }

    [HttpGet]
    [Route("{sessionId}/window_handle")]
    public IActionResult GetWindowHandle(string sessionId)
    {
      return ExecuteCommand(Command.GetWindowHandle, sessionId, null, null);
    }

    [HttpGet]
    [Route("{sessionId}/window_handles")]
    public IActionResult GetWindowHandles(string sessionId)
    {
      return ExecuteCommand(Command.GetWindowHandles, sessionId, null, null);
    }

    [HttpPost]
    [Route("{sessionId}/element/{elementId}/element")]
    public IActionResult FindElementFromElement(string sessionId, string elementId, [FromBody] object content)
    {
      return ExecuteCommand(Command.FindElement, sessionId, content, elementId);
    }

    [HttpPost]
    [Route("{sessionId}/element/{elementId}/elements")]
    public IActionResult FindElementsFromElement(string sessionId, string elementId, [FromBody] object content)
    {
      return ExecuteCommand(Command.FindElements, sessionId, content, elementId);
    }
    [HttpPost]
    [Route("{sessionId}/element/{elementId}/click")]
    public IActionResult ElementClick(string sessionId, string elementId)
    {
      return ExecuteCommand(sessionId, elementId, (element) => { element.Click(); });
    }

    [HttpPost]
    [Route("{sessionId}/element/{elementId}/doubleclick")]
    public IActionResult ElementDoubleClick(string sessionId, string elementId)
    {
      return ExecuteCommand(sessionId, elementId, (element) => { element.DoubleClick(); });
    }

    [HttpPost]
    [Route("{sessionId}/element/{elementId}/submit")]
    public IActionResult ElementSubmit(string sessionId, string elementId)
    {
      return ExecuteCommand(sessionId, elementId, (element) => { throw new Infra.NotImplementedException(); });
    }

    [HttpGet]
    [Route("{sessionId}/element/{elementId}/name")]
    public IActionResult ElementName(string sessionId, string elementId)
    {
      return ExecuteCommand(sessionId, elementId, (element) => { return element.GetName(); });
    }

    [HttpGet]
    [Route("{sessionId}/element/{elementId}/text")]
    public IActionResult ElementText(string sessionId, string elementId)
    {
      return ExecuteCommand(sessionId, elementId, (element) => { return element.GetText(); });
    }

    [HttpGet]
    [Route("{sessionId}/element/{elementId}/selected")]
    public IActionResult ElementSelected(string sessionId, string elementId)
    {
      return ExecuteCommand(sessionId, elementId, (element) => { return element.IsSelected(); });
    }

    [HttpGet]
    [Route("{sessionId}/element/{elementId}/enabled")]
    public IActionResult ElementEnabled(string sessionId, string elementId)
    {
      return ExecuteCommand(sessionId, elementId, (element) => { return element.IsEnabled(); });
    }

    [HttpGet]
    [Route("{sessionId}/element/{elementId}/displayed")]
    public IActionResult ElementDisplayed(string sessionId, string elementId)
    {
      return ExecuteCommand(sessionId, elementId, (element) => { return element.IsDisplayed(); });
    }

    [HttpGet]
    [Route("{sessionId}/element/{elementId}/location")]
    public IActionResult ElementLocation(string sessionId, string elementId)
    {
      return ExecuteCommand(sessionId, elementId, (element) => { return element.GetLocation(); });
    }

    [HttpGet]
    [Route("{sessionId}/element/{elementId}/size")]
    public IActionResult ElementSize(string sessionId, string elementId)
    {
      return ExecuteCommand(sessionId, elementId, (element) => { return element.GetSize(); });
    }

    [HttpPost]
    [Route("{sessionId}/element/{elementId}/clear")]
    public IActionResult ElementClear(string sessionId, string elementId)
    {
      return ExecuteCommand(sessionId, elementId, (element) => { element.Clear(); });
    }

    [HttpPost]
    [Route("{sessionId}/source")]
    public IActionResult GetSource(string sessionId, [FromBody] object content)
    {
      return ExecuteCommand(Command.GetSource, sessionId, content, null);
    }

    [HttpDelete]
    [Route("{sessionId}")]
    public IActionResult DeleteSession(string sessionId)
    {
      return ExecuteCommand(Command.DeleteSession, sessionId, null, null);
    }

    [HttpPost]
    [Route("{sessionId}/[action]")]
    public IActionResult Back(string sessionId, [FromBody] object content)
    {
      return Ok("Back SessionId " + sessionId + " " + content.ToString());
    }

    [HttpPost]
    [Route("{sessionId}/window")]
    public IActionResult ActivateWindow(string sessionId, [FromBody] object content)
    {
      return ExecuteCommand(Command.ActivateWindow, sessionId, content, null);
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
    [Route("{sessionId}/timeouts/async_script")]
    [Route("{sessionId}/url")]
    [Route("{sessionId}/forward")]
    [Route("{sessionId}/back")]
    [Route("{sessionId}/refresh")]
    [Route("{sessionId}/execute")]
    [Route("{sessionId}/execute_async")]
    [Route("{sessionId}/screenshot")]
    [Route("{sessionId}/ime/{any}")]
    [Route("{sessionId}/frame")]
    [Route("{sessionId}/frame/parent")]
    public IActionResult UnknownPost(string sessionId)
    {
      return ExecuteCommand(Command.UnknownCommand, sessionId, null, null);
    }

    [HttpGet]
    [Route("{sessionId}/url")]
    public IActionResult UnknownGet(string sessionId)
    {
      return ExecuteCommand(Command.UnknownCommand, sessionId, null, null);
    }
  }
}

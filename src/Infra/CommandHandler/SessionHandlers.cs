// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

using System.Linq;
using Microsoft.Windows.Apps.Test.Foundation;
using System.Collections.Generic;
using WinAppDriver.Infra.Communication;
using WinAppDriver.Infra.Result;
using System;

namespace WinAppDriver.Infra.CommandHandler
{
  class SetImplicitTimeoutHandler : SessionCommandHandlerBase<SetImplicitTimeoutReq, object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, SetImplicitTimeoutReq req, string elementId)
    {
      if (req.type != null && req.type != "implicit")
      {
        throw new InvalidArgumentException("only type=implicit is supported");
      }
      if (req.ms < 0)
      {
        throw new InvalidArgumentException("ms shoudl not be negative");
      }
      session.SetImplicitTimeout((int)req.ms);
      return null;
    }
  }

  public class GetSourceHandler : ICommandHandler
  {
    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
    {
      return sessionManager.GetSession(sessionId).GetSource();
    }
  }

  public class UknownCommandHandler : ICommandHandler
  {
    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
    {
      throw new UnknownCommandException();
    }
  }

  class DeleteSessionHandler : ICommandHandler
  {
    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
    {
      if (sessionManager.ContainsSession(sessionId))
      {
        var session = sessionManager.GetSession(sessionId);
        sessionManager.DeleteSession(sessionId);
        session.QuitApplication();
      }
      return null;
    }
  }

  class AppiumCloseAppHandler : ICommandHandler
  {
    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
    {
      if (sessionManager.ContainsSession(sessionId))
      {
        var session = sessionManager.GetSession(sessionId);
        session.QuitApplication();
      }
      return null;
    }
  }

  class AppiumLaunchAppHandler : ICommandHandler
  {
    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
    {
      var session = sessionManager.GetSession(sessionId);
      session.RelaunchApplication();
      return null;
    }
  }

  class GetWindowHandleHandler : SessionCommandHandlerBase<object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string elementId)
    {
      return session.GetWindowHandle();
    }
  }

  class ActivateWindowHandler : SessionCommandHandlerBase<ActivateWindowReq, object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, ActivateWindowReq req, string elementId)
    {
      session.GetWindow(req.name).SetFocus();
      return null;
    }
  }
  class GetCapabilitesHandler : SessionCommandHandlerBase<Capabilities>
  {
    protected override Capabilities ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string elementId)
    {
      return session.GetCapabilities();
    }
  }
  class CloseActiveWindowHandler : SessionCommandHandlerBase<object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string elementId)
    {
      session.GetApplicationRoot().CloseActiveWindow();
      return null;
    }
  }
  class GetTitleHandler : SessionCommandHandlerBase<string>
  {
    protected override string ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string elementId)
    {
      return session.GetApplicationRoot().GetTitle();
    }
  }
  class GetWindowHandlesHandler : SessionCommandHandlerBase<object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string elementId)
    {
      return session.GetWindowHandles();
    }
  }

  class GetFocusedElementHandler : SessionCommandHandlerBase<object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string elementId)
    {
      return session.GetApplicationRoot().GetFocusedElement().GetId();
    }
  }

  class SessionMoveToHandler : SessionCommandHandlerBase<MoveToReq, object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, MoveToReq req, string elementId)
    {
      if ((req.xoffset == null || req.yoffset == null) && req.element == null)
      {
        throw new InvalidArgumentException("element and xoffset/yoffset can't all be null");
      }

      IElement element = null;
      if (req.element != null)
      {
        element = session.FindElement(req.element);
      }

      element.MouseMoveTo(req.xoffset, req.yoffset);
      return null;
    }
  }

  class SessionMouseActionHandler : SessionCommandHandlerBase<MouseActionReq, object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, MouseActionReq req, string action)
    {
      req.button.MouseAction(action);
      return null;
    }
  }
  class SessionTouchActionOnElementHandler : SessionCommandHandlerBase<ElementReq, object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, ElementReq req, string action)
    {
      session.FindElement(req.element).TouchActionOnElement(action);
      return null;
    }
  }

  class SessionTouchUpDownMoveHandler : SessionCommandHandlerBase<XYReq, object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, XYReq req, string action)
    {
      req.TouchUpDownMove(action);
      return null;
    }
  }

  class GetWindowPositionHandler : SessionCommandHandlerBase<XYResult>
  {
    protected override XYResult ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string windowHandle)
    {
      return session.GetWindow(windowHandle).GetWindowPosition();
    }
  }

  class SetWindowPositionHandler : SessionCommandHandlerBase<XYReq, object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, XYReq req, string windowHandle)
    {
      session.GetWindow(windowHandle).SetWindowPosition(req);
      return null;
    }
  }

  class TakeScreenshotHandler : SessionCommandHandlerBase<string>
  {
    protected override string ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string elementId)
    {
      // bring the root window to top first
      var windowHandle = session.GetApplicationRoot().UI().NativeWindowHandle;
      Helper.Helpers.SetForegroundWindow(windowHandle);
      Helper.Helpers.BringWindowToTop(windowHandle);

      var element = elementId == null ? session.GetApplicationRoot() : session.FindElement(elementId);
      var desktopWindow = element.GetDesktopRectangle();
      var elementWindow = element.GetBoundingRectangle();
      elementWindow.IntersectsWith(desktopWindow);
      return session.TakeScreenshot(elementWindow.X, elementWindow.Y, elementWindow.Height, elementWindow.Width);
    }
  }

  class GetWindowSizeHandler : SessionCommandHandlerBase<SizeResult>
  {
    protected override SizeResult ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string windowHandle)
    {
      return session.GetWindow(windowHandle).GetWindowSize();
    }
  }

  class SetWindowSizeHandler : SessionCommandHandlerBase<SizeReq, object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, SizeReq req, string windowHandle)
    {
      session.GetWindow(windowHandle).SetWindowSize(req);
      return null;
    }
  }

  class MaximizeWindowHandler : SessionCommandHandlerBase<object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string windowHandle)
    {
      session.GetWindow(windowHandle).MaximizeWindow();
      return null;
    }
  }

  class ClickHandler : SessionCommandHandlerBase<object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string elementId)
    {
      session.FindElement(elementId).Click(Convert.ToBoolean(session.GetCapabilities().clickWithInvoke));
      return null;
    }
  }
}

// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

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

  class GetWindowHandleHandler : SessionCommandHandlerBase<object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, string elementId)
    {
      return session.GetApplicationRoot().GetWindowHandle();
    }
  }

  class ActivateWindowHandler : SessionCommandHandlerBase<ActivateWindowReq, object>
  {
    protected override object ExecuteSessionCommand(ISessionManager sessionManager, ISession session, ActivateWindowReq req, string elementId)
    {
      session.GetApplicationRoot().ActivateWindow(req.name);
      return null;
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
      return session.GetApplicationRoot().GetWindowHandles();
    }
  }
}

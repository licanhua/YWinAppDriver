using System;
// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

namespace WinAppDriver.Infra.CommandHandler
{
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
}

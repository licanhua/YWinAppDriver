// Copyright (c) https://github.com/licanhua/WinAppDriver. All rights reserved.
// Licensed under the MIT License.

namespace WinAppDriver.Infra.CommandHandler
{
  public class GetSourceHandler : ICommandHandler
  {
    public object ExecuteCommand(ISessionManager sessionManager, string sessionId, object body, string elementId)
    {
      return sessionManager.GetSession(sessionId).GetSource();
    }
  }
}

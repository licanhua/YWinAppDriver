// Copyright (c) https://github.com/licanhua/YWinAppDriver. All rights reserved.
// Licensed under the MIT License.

namespace WinAppDriver.Infra.CommandHandler
{
  public enum Command
  {
    UnknownCommand,
    NewSession,
    DeleteSession,
    GetSessions,
    FindElement,
    FindElements,
    GetSource,
    SetImplicitTimeout,
    GetStatus,
  }

  public interface ICommandHandlers
  {
    public object ExecuteCommand(Command command, ISessionManager sessionManager, string sessionId, object req, string elementId);
  }
}

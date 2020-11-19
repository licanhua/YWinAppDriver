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
    GetWindowHandle,
    GetWindowHandles,
    ActivateWindow,
    DeleteWindow,
    GetTitle,
    ElementEquals,
    GetFocusedElement,
    ElementSendKeys,
    SessionSendKeys,
    AppiumCloseApp,
    AppiumLaunchApp,
    GetCapabilities,
    SessionMoveTo,
    SessionTouchActionOnElement,
    SessionTouchUpDownMove,
    SessionMouseAction,
    SetWindowSize,
    SetWindowPosition,
    GetWindowSize,
    GetWindowPosition,
    MaximizeWindow,
    TakeScreenshot,
  }

  public interface ICommandHandlers
  {
    public object ExecuteCommand(Command command, ISessionManager sessionManager, string sessionId, object req, string elementId);
  }
}
